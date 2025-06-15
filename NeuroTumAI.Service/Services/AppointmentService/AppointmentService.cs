using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;
using NeuroTumAI.Core.Specifications.DoctorSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;
using NeuroTumAI.Service.Services.NotificationService;

namespace NeuroTumAI.Service.Services.AppointmentService
{
	public class AppointmentService : IAppointmentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILocalizationService _localizationService;
		private readonly IDoctorService _doctorService;
		private readonly INotificationService _notificationService;

		public AppointmentService(IUnitOfWork unitOfWork, ILocalizationService localizationService, IDoctorService doctorService, INotificationService notificationService)
		{
			_unitOfWork = unitOfWork;
			_localizationService = localizationService;
			_doctorService = doctorService;
			_notificationService = notificationService;
		}

		public async Task<Appointment> BookAppointmentAsync(BookAppointmentDto model, string userId)
		{
			if (model.Date < DateOnly.FromDateTime(DateTime.Today))
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("InvalidDate"));

			var slotRepo = _unitOfWork.Repository<Slot>();
			var slot = await slotRepo.GetAsync(model.SlotId);

			if (slot is null || !slot.IsAvailable)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("SlotNotFound"));

			if (slot.DayOfWeek != model.Date.DayOfWeek)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("InvalidDate"));

			var clinicRepo = _unitOfWork.Repository<Clinic>();

			var clinic = await clinicRepo.GetAsync(slot.ClinicId);
			if (!clinic.IsApproved)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("SlotNotFound"));

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpec = new AppointmentSpecifications(slot.StartTime, model.Date, slot.ClinicId);
			var existAppointment = await appointmentRepo.GetWithSpecAsync(appointmentSpec);

			if (existAppointment is not null)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("AppointmentExists"));


			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpec = new PatientSpecifications(userId);
			var patient = await patientRepo.GetWithSpecAsync(patientSpec);

			var newAppointment = new Appointment()
			{
				Date = model.Date,
				DoctorId = clinic.DoctorId,
				PatientId = patient.Id,
				StartTime = slot.StartTime,
				ClinicId = slot.ClinicId,
			};

			appointmentRepo.Add(newAppointment);

			await _unitOfWork.CompleteAsync();

			var newAppointmentNotification = new NewAppointmentNotificationDto()
			{
				Date = newAppointment.Date,
				Time = newAppointment.StartTime,
				DoctorId = clinic.DoctorId
			};

			await _notificationService.SendNewAppointmentNotificationAsync(newAppointmentNotification);

			return newAppointment;
		}

		public async Task<Appointment> CancelAppointmentAsync(string userId, int appointmentId)
		{
			var appointmentSpecs = new AppointmentSpecifications(appointmentId);

			var appoitnmentRepo = _unitOfWork.Repository<Appointment>();

			var appointment = await appoitnmentRepo.GetWithSpecAsync(appointmentSpecs);
			if (appointment is null || appointment.Status != AppointmentStatus.Pending || (appointment.Doctor.ApplicationUserId != userId && appointment.Patient.ApplicationUserId != userId))
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("appointmentCancelInvalid"));

			appointment.Status = AppointmentStatus.Cancelled;
			appoitnmentRepo.Update(appointment);


			var notifications = new List<AppointmentCancellationNotificationDto>();


			var newNotification = new AppointmentCancellationNotificationDto()
			{
				Date = appointment.Date,
				PatientId = appointment.PatientId,
				DoctorId = appointment.DoctorId
			};

			notifications.Add(newNotification);


			await _unitOfWork.CompleteAsync();

			await _notificationService.SendAppointmentCancellationNotificationsAsync(notifications);

			return appointment;
		}

		public async Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(string userId, int clinicId, DateOnly date)
		{
			var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinic = await clinicRepo.GetAsync(clinicId);
			if (clinic is null || clinic.DoctorId != doctor.Id)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpecs = new AppointmentWithPatientSpecifications(date, clinicId);
			var appointments = await appointmentRepo.GetAllWithSpecAsync(appointmentSpecs);

			return appointments;

		}

		public async Task<IReadOnlyList<Appointment>> GetPatientAppointmentsAsync(string userId, AppointmentSpecParams specParams)
		{
			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpec = new PatientSpecifications(userId);
			var patient = await patientRepo.GetWithSpecAsync(patientSpec);

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpecs = new PatientAppointmentSpecifications(specParams, patient.Id);

			return await appointmentRepo.GetAllWithSpecAsync(appointmentSpecs);
		}

		public async Task<int> GetPatientAppointmentsCountAsync(string userId, AppointmentSpecParams specParams)
		{
			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpec = new PatientSpecifications(userId);
			var patient = await patientRepo.GetWithSpecAsync(patientSpec);

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpecs = new PatientAppointmentCountSpecifications(specParams, patient.Id);

			return await appointmentRepo.GetCountAsync(appointmentSpecs);
		}
	}
}
