using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Dtos.ContactUs;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;
using NeuroTumAI.Service.Services.ContactUsService;

namespace NeuroTumAI.APIs.Controllers.Appointment
{
	public class AppointmentController : BaseApiController
	{
		private readonly IAppointmentService _appointmentService;
		private readonly IMapper _mapper;

		public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
		{
			_appointmentService = appointmentService;
			_mapper = mapper;
		}

		[Authorize(Roles = "Patient")]
		[HttpPost]
		public async Task<ActionResult<AppointmentToReturnDto>> BookAppointemnt([FromBody] BookAppointmentDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var appointment = await _appointmentService.BookAppointmentAsync(model, userId);

			return Ok(_mapper.Map<AppointmentToReturnDto>(appointment));
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpGet("doctor/{clinicId}")]
		public async Task<ActionResult<IReadOnlyList<AppoitntmentWithPatientDto>>> GetDoctorAppointments(int clinicId, [FromQuery] DateOnly date)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var appointments = await _appointmentService.GetDoctorAppointmentsAsync(userId, clinicId, date);

			return Ok(_mapper.Map<IReadOnlyList<AppoitntmentWithPatientDto>>(appointments));
		}

		[Authorize(Roles = "Patient")]
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<PaginationDto<AppointmentWithDoctorDto>>>> GetPatientAppointments([FromQuery] AppointmentSpecParams specParams)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var appointments = await _appointmentService.GetPatientAppointmentsAsync(userId, specParams);
			var count = await _appointmentService.GetPatientAppointmentsCountAsync(userId, specParams);

			var data = _mapper.Map<IReadOnlyList<AppointmentWithDoctorDto>>(appointments);

			return Ok(new PaginationDto<AppointmentWithDoctorDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}

		[Authorize]
		[HttpPatch("cancel/{appointmentId}")]
		public async Task<ActionResult> CancelAppointment(int appointmentId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			await _appointmentService.CancelAppointmentAsync(userId, appointmentId);

			return Ok(new
			{
				Message = "Appointment has been successfully cancelled."
			});
		}
	}
}
