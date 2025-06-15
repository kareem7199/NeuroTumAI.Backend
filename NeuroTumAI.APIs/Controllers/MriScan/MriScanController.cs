using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Dtos.MriScan;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.MriScan
{
	public class MriScanController : BaseApiController
	{
		private readonly IMriScanService _mriScanService;
		private readonly IMapper _mapper;

		public MriScanController(IMriScanService mriScanService, IMapper mapper)
		{
			_mriScanService = mriScanService;
			_mapper = mapper;
		}

		[Authorize(Roles = "Patient")]
		[HttpPost("upload")]
		public async Task<ActionResult> UploadMriScan([FromForm] PredictRequestDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var result = await _mriScanService.UploadAndProcessMriScanAsync(model, userId);

			return Ok(new { result.Id });
		}

		[Authorize(Roles = "Doctor")]
		[HttpGet("assignedScans")]
		public async Task<ActionResult<PaginationDto<MriScanResultToReviewDto>>> GetAssignedMriScans([FromQuery] PaginationParamsDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

			var scans = await _mriScanService.GetAssignedScansAsync(userId, dto);
			var count = await _mriScanService.GetAssignedScansCountAsync(userId);

			return Ok(new PaginationDto<MriScanResultToReviewDto>(dto.PageIndex, dto.PageSize, count, _mapper.Map<IReadOnlyList<MriScanResultToReviewDto>>(scans)));
		}


		[Authorize(Roles = "Doctor")]
		[HttpPost("review/{mriScanId}")]
		public async Task<ActionResult> ReviewAsync(int mriScanId, [FromBody] AddMriScanReviewDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

			await _mriScanService.ReviewAsync(mriScanId, userId, model);

			return Ok(new
			{
				Message = "MRI scan has been reviewed successfully."
			});
		}

		[Authorize(Roles = "Patient")]
		[HttpGet]
		public async Task<ActionResult<PaginationDto<PatientMriScanDto>>> GetPatientMriScans([FromQuery] PaginationParamsDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

			var scans = await _mriScanService.GetPatientScansAsync(userId, dto);
			var count = await _mriScanService.GetPatientScansCountAsync(userId);

			return Ok(new PaginationDto<PatientMriScanDto>(dto.PageIndex, dto.PageSize, count, _mapper.Map<IReadOnlyList<PatientMriScanDto>>(scans)));
		}
	}
}
