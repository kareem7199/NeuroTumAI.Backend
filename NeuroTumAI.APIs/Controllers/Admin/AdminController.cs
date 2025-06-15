using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Admin;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Dtos.ContactUs;
using NeuroTumAI.Core.Dtos.Doctor;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.ContactUsMessageSpecs;
using NeuroTumAI.Core.Specifications.DoctorSpecs;

namespace NeuroTumAI.APIs.Controllers.Admin
{
	public class AdminController : BaseApiController
	{
		private readonly IAdminService _adminService;
		private readonly IMapper _mapper;
		private readonly IDoctorService _doctorService;
		private readonly IClinicService _clinicService;
		private readonly IDashboardService _dashboardService;
		private readonly IContactUsService _contactUsService;

		public AdminController(IAdminService adminService, IMapper mapper, IDoctorService doctorService, IClinicService clinicService, IDashboardService dashboardService, IContactUsService contactUsService)
		{
			_adminService = adminService;
			_mapper = mapper;
			_doctorService = doctorService;
			_clinicService = clinicService;
			_dashboardService = dashboardService;
			_contactUsService = contactUsService;
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginDto loginDto)
		{
			var token = await _adminService.LoginAdminAsync(loginDto);

			return Ok(new { Data = token });
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult> GetAdmin()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var intUserId = int.Parse(userId);

			var admin = await _adminService.GetAdminByIdAsync(intUserId);
			if (admin is null)
				throw new UnAuthorizedException("User not found.");


			return Ok(new { Data = _mapper.Map<AdminToReturnDto>(admin) });
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("pendingDoctors")]
		public async Task<ActionResult> GetPendingDoctors([FromQuery] PendingDoctorSpecParams specParams)
		{
			var doctors = await _doctorService.GetPendingDoctorsAsync(specParams);
			var count = await _doctorService.GetPendingDoctorsCountAsync(specParams);

			var data = _mapper.Map<IReadOnlyList<PendingDoctorDto>>(doctors);

			return Ok(new PaginationDto<PendingDoctorDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("pendingDoctors/accept/{doctorId}")]
		public async Task<ActionResult> AcceptPendingDoctor(int doctorId)
		{
			await _doctorService.AcceptPendingDoctorAsync(doctorId);

			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("pendingDoctors/reject/{doctorId}")]
		public async Task<ActionResult> RejectPendingDoctor(int doctorId)
		{
			await _doctorService.RejectPendingDoctorAsync(doctorId);

			return Ok();
		}


		[Authorize(Roles = "Admin")]
		[HttpGet("pendingClinics")]
		public async Task<ActionResult> GetPendingClinics([FromQuery] PendingClinicSpecParams specParams)
		{
			var clinics = await _clinicService.GetPendingClinicsAsync(specParams);
			var count = await _clinicService.GetPendingClinicsCountAsync(specParams);

			var data = _mapper.Map<IReadOnlyList<PendingClinicDto>>(clinics);

			return Ok(new PaginationDto<PendingClinicDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("pendingClinics/accept/{clinicId}")]
		public async Task<ActionResult> AcceptPendingClinic(int clinicId)
		{
			await _clinicService.AcceptPendingClinicAsync(clinicId);

			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("pendingClinics/reject/{clinicId}")]
		public async Task<ActionResult> RejectPendingClinic(int clinicId)
		{
			await _clinicService.RejectPendingClinicAsync(clinicId);

			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("stats")]
		public async Task<ActionResult> GetStatistics()
		{
			var stats = await _dashboardService.GetStatisticsAsync();

			return Ok(new { Data = stats });
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("contactUsMessages")]
		public async Task<ActionResult> GetPendingMessages([FromQuery] PendingMessagesSpecParams specParams)
		{
			var messages = await _contactUsService.GetPendingMessagesAsync(specParams);
			var count = await _contactUsService.GetPendingMessagesCountAsync(specParams);

			var data = _mapper.Map<IReadOnlyList<ContactUsMessageToReturnDto>>(messages);

			return Ok(new PaginationDto<ContactUsMessageToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("contactUsMessages/{messageId}")]
		public async Task<ActionResult> GetMessageById(int messageId)
		{
			var message = await _contactUsService.GetMessageAsync(messageId);

			return Ok(new { Data = _mapper.Map<ContactUsMessageToReturnDto>(message) });
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("contactUsMessages/{messageId}/reply")]
		public async Task<ActionResult> Reply(int messageId,[FromBody] ContactUsDto model)
		{
			await _contactUsService.ReplyAsync(messageId, model.Message);

			return Ok();
		}
	}
}
