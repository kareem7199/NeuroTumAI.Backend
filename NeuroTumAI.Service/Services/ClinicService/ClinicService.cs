using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.DoctorSpecs;
using NeuroTumAI.Core.Specifications.SlotSpecs;

namespace NeuroTumAI.Service.Services.ClinicService
{
	public class ClinicService : IClinicService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILocalizationService _localizationService;
		private readonly IBlobStorageService _blobStorageService;
		private readonly INotificationService _notificationService;

		public ClinicService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IBlobStorageService blobStorageService, INotificationService notificationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizationService = localizationService;
			_blobStorageService = blobStorageService;
			_notificationService = notificationService;
		}

		public async Task<Clinic> AcceptPendingClinicAsync(int clinicId)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinic = await clinicRepo.GetAsync(clinicId);
			if (clinic is null)
				throw new NotFoundException($"Clinic with the specified ID was not found.");

			clinic.IsApproved = true;

			await _unitOfWork.CompleteAsync();

			return clinic;
		}

		public async Task<Clinic> AddClinicAsync(BaseAddClinicDto model, string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			using var clinicLicenseDocumentStream = model.LicenseDocument.OpenReadStream();
			var clinicLicenseDocument = await _blobStorageService.UploadFileAsync(clinicLicenseDocumentStream, model.LicenseDocument.FileName, "clinic-licenses");

			var location = new NetTopologySuite.Geometries.Point(model.Longitude, model.Latitude) { SRID = 4326 };

			var newClinic = new Clinic()
			{
				Address = model.Address,
				Location = location,
				PhoneNumber = model.PhoneNumber,
				DoctorId = doctor.Id,
				LicenseDocument = clinicLicenseDocument
			};

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			clinicRepo.Add(newClinic);

			await _unitOfWork.CompleteAsync();

			return newClinic;
		}

		public async Task<Slot> AddSlotAsync(AddSlotDto slot, string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var newSlot = _mapper.Map<Slot>(slot);

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinic = await clinicRepo.GetAsync(newSlot.ClinicId);

			if (clinic is null || clinic.DoctorId != doctor.Id)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var slotRepo = _unitOfWork.Repository<Slot>();
			var slotSpecs = new SlotSpecifications(newSlot.DayOfWeek, newSlot.StartTime, clinic.Id);
			var existedSlot = await slotRepo.GetWithSpecAsync(slotSpecs);

			if (existedSlot is not null)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("SlotAlreadyExists"));

			slotRepo.Add(newSlot);

			await _unitOfWork.CompleteAsync();

			return newSlot;
		}

		public async Task DeleteSlotAsync(string userId, int slotId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var slotRepo = _unitOfWork.Repository<Slot>();
			var slot = await slotRepo.GetAsync(slotId);
			if (slot is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("SlotNotFound"));

			var clinic = await GetClinicByIdAsync(slot.ClinicId);
			if (clinic is null || clinic.DoctorId != doctor.Id)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("SlotNotFound"));

			var upComingDates = GetUpcomingDatesForDay(slot.DayOfWeek, 120, DateTime.Today.AddDays(-1));

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpecs = new AppointmentSpecifications(upComingDates, AppointmentStatus.Pending);
			var appointments = await appointmentRepo.GetAllWithSpecAsync(appointmentSpecs);

			var notifications = new List<AppointmentCancellationNotificationDto>();

			foreach ( var appointment in appointments)
			{
				appointment.Status = AppointmentStatus.Cancelled;
				appointmentRepo.Update(appointment);

				var newNotification = new AppointmentCancellationNotificationDto()
				{
					Date = appointment.Date,
					PatientId = appointment.PatientId,
					DoctorId = appointment.DoctorId
				};

				notifications.Add(newNotification);
			}

			slotRepo.Delete(slot);


			await _unitOfWork.CompleteAsync();

			await _notificationService.SendAppointmentCancellationNotificationsAsync(notifications);
		}

		public async Task<IReadOnlyList<Slot>> GetClinicAvailableSlotsAsync(int clinicId, DateOnly date)
		{
			var clinic = await GetClinicByIdAsync(clinicId);
			if (clinic is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var day = date.DayOfWeek;

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpec = new AppointmentSpecifications(date, clinicId);
			var appointments = await appointmentRepo.GetAllWithSpecAsync(appointmentSpec);

			var times = new List<TimeOnly>();

			foreach (var appointment in appointments)
				times.Add(appointment.StartTime);

			var slotRepo = _unitOfWork.Repository<Slot>();
			var slotSpecs = new AvailableSlotSpecifications(day, clinicId, times);
			var slots = await slotRepo.GetAllWithSpecAsync(slotSpecs);

			return slots;
		}

		public async Task<Clinic?> GetClinicByIdAsync(int clinicId)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			return await clinicRepo.GetAsync(clinicId);
		}

		public async Task<IReadOnlyList<Clinic>> GetClinicsAsync(ClinicSpecParams model)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpec = new NearbyClinicsSpecifications(model);
			var clinics = await clinicRepo.GetAllWithSpecAsync(clinicSpec);

			return clinics;
		}

		public async Task<IReadOnlyList<Slot>> GetClinicSlotsAsync(string userId, int clinicId, DayOfWeek day)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinic = await clinicRepo.GetAsync(clinicId);

			if (clinic is null || clinic.DoctorId != doctor.Id)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var slotaRepo = _unitOfWork.Repository<Slot>();
			var slotSpecs = new SlotSpecifications(day, clinicId);
			var slots = await slotaRepo.GetAllWithSpecAsync(slotSpecs);

			return slots;
		}

		public async Task<int> GetCountAsync(ClinicSpecParams specParams)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpec = new NearbyClinicsForCountSpecifications(specParams);

			var count = await clinicRepo.GetCountAsync(clinicSpec);

			return count;
		}

		public async Task<IReadOnlyList<Clinic>> GetDoctorClinicAsync(string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpecs = new ClinicSpecifications(doctor.Id);

			var clinics = await clinicRepo.GetAllWithSpecAsync(clinicSpecs);

			return clinics;
		}

		public async Task<IReadOnlyList<Clinic>> GetPendingClinicsAsync(PendingClinicSpecParams specParams)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpecs = new PendingClinicSpecifications(specParams);

			return await clinicRepo.GetAllWithSpecAsync(clinicSpecs);
		}

		public async Task<int> GetPendingClinicsCountAsync(PendingClinicSpecParams specParams)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpecs = new PendingClinicCountSpecifications(specParams);

			return await clinicRepo.GetCountAsync(clinicSpecs);
		}

		public async Task RejectPendingClinicAsync(int clinicId)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinic = await clinicRepo.GetAsync(clinicId);
			if (clinic is null)
				throw new NotFoundException($"Clinic with the specified ID was not found.");

			clinicRepo.Delete(clinic);

			await _unitOfWork.CompleteAsync();
		}

		public static List<DateOnly> GetUpcomingDatesForDay(DayOfWeek targetDay, int count, DateTime? startDate = null)
		{
			var result = new List<DateOnly>();
			var start = startDate ?? DateTime.Today;

			int daysUntilTarget = ((int)targetDay - (int)start.DayOfWeek + 7) % 7;
			var current = start.AddDays(daysUntilTarget == 0 ? 7 : daysUntilTarget);

			for (int i = 0; i < count; i++)
			{
				result.Add(DateOnly.FromDateTime(current));
				current = current.AddDays(7);
			}

			return result;
		}

		public async Task UpdateSlotTimeAsync(string userId, int slotId, TimeOnly time)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var slotRepo = _unitOfWork.Repository<Slot>();
			var slot = await slotRepo.GetAsync(slotId);
			if (slot is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("SlotNotFound"));

			var clinic = await GetClinicByIdAsync(slot.ClinicId);
			if (clinic is null || clinic.DoctorId != doctor.Id)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("SlotNotFound"));

			var upComingDates = GetUpcomingDatesForDay(slot.DayOfWeek, 120, DateTime.Today.AddDays(-1));

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpecs = new AppointmentSpecifications(upComingDates, AppointmentStatus.Pending, slot.StartTime);
			var appointments = await appointmentRepo.GetAllWithSpecAsync(appointmentSpecs);

			var notifications = new List<AppointmentTimeChangeNotificationDto>();

			foreach (var appointment in appointments)
			{
				var newNotification = new AppointmentTimeChangeNotificationDto()
				{
					Date = appointment.Date,
					PatientId = appointment.PatientId,
					OldTime = appointment.StartTime
				};

				notifications.Add(newNotification);

				appointment.StartTime = time;
				appointmentRepo.Update(appointment);
			}

			slot.StartTime = time;
			slotRepo.Update(slot);


			await _unitOfWork.CompleteAsync();

			await _notificationService.SendAppointmentTimeChangeNotificationsAsync(notifications, time);
		}
	}
}
