using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Review
{
	public class ReviewController : BaseApiController
	{
		private readonly IReviewService _reviewService;
		private readonly IMapper _mapper;

		public ReviewController(IReviewService reviewService, IMapper mapper)
		{
			_reviewService = reviewService;
			_mapper = mapper;
		}

		[HttpPost]
		[Authorize(Roles = "Patient")]
		public async Task<ActionResult<ReviewToReturnDto>> AddReview(AddReviewDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var review = await _reviewService.AddReviewAsync(model, userId);
			return Ok(_mapper.Map<ReviewToReturnDto>(review));
		}

		[HttpGet("{doctorId}")]
		[Authorize(Roles = "Patient")]
		public async Task<ActionResult<PaginationDto<ReviewToReturnDto>>> GetDoctorReviews(int doctorId, [FromQuery] PaginationParamsDto specParams)
		{
			var reviews = await _reviewService.GetDoctorReviewsAsync(doctorId, specParams);
			var count = await _reviewService.GetDoctorReviewsCountAsync(doctorId);

			return Ok(new PaginationDto<ReviewToReturnDto>(specParams.PageIndex, specParams.PageSize, count, _mapper.Map<IReadOnlyList<ReviewToReturnDto>>(reviews)));
		}
	}
}
