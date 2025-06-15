using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Dtos.MriScan;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.MriScan;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.DoctorMriAssignmentsSpecs;
using NeuroTumAI.Core.Specifications.MriScanSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;

namespace NeuroTumAI.Service.Services.MriScanService
{
	public class MriScanService : IMriScanService
	{
		private readonly IBlobStorageService _blobStorageService;
		private readonly ICancerDetectionService _cancerDetectionService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDoctorService _doctorService;
		private readonly ILocalizationService _localizationService;
		private readonly INotificationService _notificationService;

		public MriScanService(IBlobStorageService blobStorageService, ICancerDetectionService cancerDetectionService, IUnitOfWork unitOfWork, IDoctorService doctorService, ILocalizationService localizationService, INotificationService notificationService)
		{
			_blobStorageService = blobStorageService;
			_cancerDetectionService = cancerDetectionService;
			_unitOfWork = unitOfWork;
			_doctorService = doctorService;
			_localizationService = localizationService;
			_notificationService = notificationService;
		}

		public async Task AutoReviewAsync(int mriId)
		{
			var mriSpecs = new MriScanSpecifications(mriId);

			var mriScanRepository = _unitOfWork.Repository<MriScan>();

			var mriScan = await mriScanRepository.GetWithSpecAsync(mriSpecs);
			mriScan.IsReviewed = true;

			var doctorAssignmentsRepository = _unitOfWork.Repository<DoctorMriAssignment>();

			mriScanRepository.Update(mriScan);
			doctorAssignmentsRepository.RemoveRange(mriScan.DoctorAssignments);

			await _unitOfWork.CompleteAsync();

			await _notificationService.SendReadyMriScanNotificationAsync(mriScan.PatientId);
		}

		public async Task<IReadOnlyList<DoctorMriAssignment>> GetAssignedScansAsync(string userId, PaginationParamsDto dto)
		{
			var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);

			var mriAssignmentSpecs = new DoctorMriAssignmentsSpecifications(doctor.Id, dto);

			return await _unitOfWork.Repository<DoctorMriAssignment>().GetAllWithSpecAsync(mriAssignmentSpecs);
		}

		public async Task<int> GetAssignedScansCountAsync(string userId)
		{
			var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);

			var mriAssignmentSpecs = new DoctorMriAssignmentsSpecifications(doctor.Id);

			return await _unitOfWork.Repository<DoctorMriAssignment>().GetCountAsync(mriAssignmentSpecs);
		}

		public async Task<IReadOnlyList<MriScan>> GetExpiredUnreviewedScansAsync()
		{
			var twelveHoursAgo = DateTime.UtcNow.AddHours(-12);
			var mriScansSpec = new MriScanSpecifications(twelveHoursAgo);

			return await _unitOfWork.Repository<MriScan>().GetAllWithSpecAsync(mriScansSpec);
		}

		public async Task<IReadOnlyList<MriScan>> GetPatientScansAsync(string userId, PaginationParamsDto dto)
		{
			var patientSpecs = new PatientSpecifications(userId);
			var patient = await _unitOfWork.Repository<Patient>().GetWithSpecAsync(patientSpecs);

			var mriScanSpecs = new PatientMriScanSpecifications(patient.Id, dto);

			return await _unitOfWork.Repository<MriScan>().GetAllWithSpecAsync(mriScanSpecs);
		}

		public async Task<int> GetPatientScansCountAsync(string userId)
		{
			var patientSpecs = new PatientSpecifications(userId);
			var patient = await _unitOfWork.Repository<Patient>().GetWithSpecAsync(patientSpecs);

			var mriScanSpecs = new PatientMriScanSpecifications(patient.Id);

			return await _unitOfWork.Repository<MriScan>().GetCountAsync(mriScanSpecs);
		}

		public async Task ReviewAsync(int mriScanId, string userId, AddMriScanReviewDto reviewDto)
		{
			var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);

			var mriScanSpecs = new MriScanSpecifications(mriScanId);
			var mriScanRepo = _unitOfWork.Repository<MriScan>();

			var mriScan = await mriScanRepo.GetWithSpecAsync(mriScanSpecs);

			if (mriScan is null || mriScan.IsReviewed)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("MriScan_NotFoundOrReviewed"));

			var isAssigned = mriScan.DoctorAssignments.Any(DA => DA.DoctorId == doctor.Id);
			if (!isAssigned)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("MriScan_NotFoundOrReviewed"));

			mriScan.IsReviewed = true;
			mriScan.DetectionClass = reviewDto.DetectionClass;

			var newReview = new DoctorReview()
			{
				Findings = reviewDto.Findings,
				Reasoning = reviewDto.Reasoning,
				DoctorId = doctor.Id,
			};

			mriScan.DoctorReview = newReview;

			mriScan.DoctorAssignments = new HashSet<DoctorMriAssignment>();

			mriScanRepo.Update(mriScan);

			await _unitOfWork.CompleteAsync();

			await _notificationService.SendReadyMriScanNotificationAsync(mriScan.PatientId);
		}

		public async Task<MriScan> UploadAndProcessMriScanAsync(PredictRequestDto model, string userId)
		{
			using var stream = model.Image.OpenReadStream();
			var fileUrl = await _blobStorageService.UploadFileAsync(stream, model.Image.FileName, "patient-cancer-images");

			var aiResponse = await _cancerDetectionService.PredictCancerAsync(fileUrl);

			var nearbyClinicSpecParams = new ClinicSpecParams
			{
				Latitude = model.Latitude,
				Longitude = model.Longitude,
				PageSize = 15,
				PageIndex = 1
			};

			var clinicSpecs = new NearbyClinicsSpecifications(nearbyClinicSpecParams);
			var nearbyClinics = await _unitOfWork.Repository<Clinic>().GetAllWithSpecAsync(clinicSpecs);

			var patientSpecs = new PatientSpecifications(userId);
			var patient = await _unitOfWork.Repository<Patient>().GetWithSpecAsync(patientSpecs);


			var newMriScan = new MriScan()
			{
				Confidence = aiResponse.Confidence,
				AiGeneratedImagePath = aiResponse.Image,
				DetectionClass = aiResponse.Class,
				PatientId = patient.Id,
				ImagePath = fileUrl
			};


			var doctorIds = new List<int>();
			foreach (var nearbyClinic in nearbyClinics)
			{
				var doctorId = nearbyClinic.Doctor.Id;

				if (!newMriScan.DoctorAssignments.Any(DC => DC.DoctorId == doctorId))
				{
					var newDoctorAssignment = new DoctorMriAssignment()
					{
						DoctorId = doctorId,
					};

					newMriScan.DoctorAssignments.Add(newDoctorAssignment);
					doctorIds.Add(nearbyClinic.Doctor.Id);
				}

				if (newMriScan.DoctorAssignments.Count == 2) break;
			}

			_unitOfWork.Repository<MriScan>().Add(newMriScan);

			await _unitOfWork.CompleteAsync();

			await _notificationService.SendMriScanAssignmentNotificationToDoctorAsync(doctorIds);

			return newMriScan;
		}
	}
}
