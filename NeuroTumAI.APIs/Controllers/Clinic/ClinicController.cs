using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.ClinicSpecs;

namespace NeuroTumAI.APIs.Controllers.Clinic
{
	public class ClinicController : BaseApiController
	{
		private readonly IClinicService _clinicService;
		private readonly IMapper _mapper;

		public ClinicController(IClinicService clinicService, IMapper mapper)
		{
			_clinicService = clinicService;
			_mapper = mapper;
		}

		[Authorize(Roles = "Patient")]
		[HttpGet]
		public async Task<ActionResult<PaginationDto<ClinicWithDoctorDataDto>>> GetClinics([FromQuery] ClinicSpecParams specParams)
		{
			var clinics = await _clinicService.GetClinicsAsync(specParams);
			var count = await _clinicService.GetCountAsync(specParams);

			var data = _mapper.Map<IReadOnlyList<ClinicWithDoctorDataDto>>(clinics);

			return Ok(new PaginationDto<ClinicWithDoctorDataDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpGet("doctor")]
		public async Task<ActionResult<IReadOnlyList<ClinicToReturnDto>>> GetDoctorClinics()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var clinics = await _clinicService.GetDoctorClinicAsync(userId);

			return Ok(_mapper.Map<IReadOnlyList<ClinicToReturnDto>>(clinics));
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpGet("doctor/{clinicId}/slots")]
		public async Task<ActionResult<IReadOnlyList<SlotToReturnDto>>> GetDoctorClinicSlots(int clinicId, [FromQuery] DayOfWeek day)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var slots = await _clinicService.GetClinicSlotsAsync(userId, clinicId, day);

			return Ok(_mapper.Map<IReadOnlyList<SlotToReturnDto>>(slots));
		}


		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpPost]
		public async Task<ActionResult<ClinicToReturnDto>> AddClinic([FromForm] BaseAddClinicDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var clinic = await _clinicService.AddClinicAsync(model, userId);

			return Ok(_mapper.Map<ClinicToReturnDto>(clinic));
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpPost("slot")]
		public async Task<ActionResult<SlotToReturnDto>> AddSlot([FromBody] AddSlotDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var slot = await _clinicService.AddSlotAsync(model, userId);

			return Ok(_mapper.Map<SlotToReturnDto>(slot));

		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpDelete("slot/{slotId}")]
		public async Task<ActionResult> DeleteSlot(int slotId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			await _clinicService.DeleteSlotAsync(userId, slotId);

			return Ok(new
			{
				Message = "Slot deleted successfully."
			});
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpPut("slot/{slotId}")]
		public async Task<ActionResult> UpdateSlotTime(int slotId, [FromBody] UpdateSlotTimeDto updateSlotTimeDto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			await _clinicService.UpdateSlotTimeAsync(userId, slotId, updateSlotTimeDto.StartTime);

			return Ok(new
			{
				Message = "Slot time updated successfully."
			});
		}

		[Authorize(Roles = "Patient")]
		[HttpGet("availableSlots/{clinicId}")]
		public async Task<ActionResult<IReadOnlyList<SlotToReturnDto>>> GetClinicAvailableSlots([FromQuery] GetAvailableSlotsDto model, int clinicId)
		{
			var slots = await _clinicService.GetClinicAvailableSlotsAsync(clinicId, model.Date);

			return Ok(_mapper.Map<IReadOnlyList<SlotToReturnDto>>(slots));

		}
	}
}
