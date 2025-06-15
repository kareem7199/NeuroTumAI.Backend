using Microsoft.AspNetCore.Identity;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.DoctorSpecs;

namespace NeuroTumAI.Service.Services.DoctorService
{
	public class DoctorService : IDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IClinicService _clinicService;
		private readonly ILocalizationService _localizationService;
		private readonly UserManager<ApplicationUser> _userManager;

		public DoctorService(IUnitOfWork unitOfWork, IClinicService clinicService, ILocalizationService localizationService, UserManager<ApplicationUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_clinicService = clinicService;
			_localizationService = localizationService;
			_userManager = userManager;
		}
		public async Task<Doctor> GetDoctorByClinicIdAsync(int clinicId)
		{
			var clinic = await _clinicService.GetClinicByIdAsync(clinicId);
			if (clinic is null || !clinic.IsApproved)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(clinic.DoctorId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			return doctor!;
		}

		public async Task<Doctor> GetDoctorByUserIdAsync(string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			return await doctorRepo.GetWithSpecAsync(doctorSpec);
		}

		public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			return await doctorRepo.GetAsync(doctorId);
		}

		public async Task<IReadOnlyList<Doctor>> GetPendingDoctorsAsync(PendingDoctorSpecParams model)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpecs = new PendingDoctorSpecifications(model);

			return await doctorRepo.GetAllWithSpecAsync(doctorSpecs);
		}

		public async Task<int> GetPendingDoctorsCountAsync(PendingDoctorSpecParams model)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpecs = new PendingDoctorCountSpecifications(model);

			return await doctorRepo.GetCountAsync(doctorSpecs);
		}

		public async Task<Doctor> AcceptPendingDoctorAsync(int doctorId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();

			var doctor = await doctorRepo.GetAsync(doctorId);
			if (doctor is null)
				throw new NotFoundException($"Doctor with ID {doctorId} was not found.");

			doctor.IsApproved = true;

			doctorRepo.Update(doctor);
			
			await _unitOfWork.CompleteAsync();

			return doctor;
		}

		public async Task RejectPendingDoctorAsync(int doctorId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();

			var doctor = await doctorRepo.GetAsync(doctorId);
			if (doctor is null)
				throw new NotFoundException($"Doctor with ID {doctorId} was not found.");

			var user = await _userManager.FindByIdAsync(doctor.ApplicationUserId);
			var result = await _userManager.DeleteAsync(user);

			if (!result.Succeeded)
				throw new BadRequestException("Failed to delete doctor");
		}

		public async Task<Doctor> GetDoctorProfileAsync(string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorProfileSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			if(doctor is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("UserNotFound"));

			doctor.Clinics = doctor.Clinics.Where(C => C.IsApproved).ToList();

			return doctor!;
		}
	}
}
