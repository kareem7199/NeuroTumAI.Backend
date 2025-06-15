using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Doctor;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Doctor
{
	public class DoctorController : BaseApiController
	{
		private readonly IDoctorService _doctorService;
		private readonly IReviewService _reviewService;
		private readonly IMapper _mapper;

		public DoctorController(IDoctorService doctorService, IReviewService reviewService , IMapper mapper)
		{
			_doctorService = doctorService;
			_reviewService = reviewService;
			_mapper = mapper;
		}

		[Authorize(Roles = "Patient")]
		[HttpGet("byClinic/{clinicId}")]
		public async Task<ActionResult<DoctorWithReviewsDto>> GetDoctorByClinicId(int clinicId)
		{
			var doctor = await _doctorService.GetDoctorByClinicIdAsync(clinicId);

			var latest5Reviews = await _reviewService.GetDoctorLatest5ReviewsAsync(doctor.Id);

			var doctorDto = _mapper.Map<DoctorWithReviewsDto>(doctor);
			var reviewsDto = _mapper.Map<IReadOnlyList<ReviewToReturnDto>>(latest5Reviews);

			doctorDto.Reviews = reviewsDto;

			return Ok(doctorDto);
		}

		[Authorize(Roles = "Patient")]
		[HttpGet("{doctorId}")]
		public async Task<ActionResult<DoctorProfileDto>> GetDoctorProfile(string doctorId)
		{
			var doctor = await _doctorService.GetDoctorProfileAsync(doctorId);

			var doctorDto = _mapper.Map<DoctorProfileDto>(doctor);

			return Ok(doctorDto);
		}

	}
}
