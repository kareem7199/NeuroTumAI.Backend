using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.APIs.Controllers.Auth
{
	public class AuthController : BaseApiController
	{
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;

		public AuthController(IMapper mapper, IAccountService accountService)
		{
			_mapper = mapper;
			_accountService = accountService;
		}

		[HttpPost("register/patient")]
		public async Task<ActionResult<RegisterResponseDto>> RegisterPatient([FromForm] PatientRegisterDto model)
		{
			var newPatient = await _accountService.RegisterPatientAsync(model);

			return Ok(new RegisterResponseDto() { Email = newPatient.ApplicationUser.Email! });
		}

		[HttpPost("register/doctor")]
		public async Task<ActionResult<RegisterResponseDto>> RegisterDoctor([FromForm] RegisterDoctorWithClinicDto model)
		{
			var newDoctor = await _accountService.RegisterDoctorAsync(model);

			return Ok(new RegisterResponseDto() { Email = newDoctor.ApplicationUser.Email! });
		}

		[HttpPost("verifyEmail")]
		public async Task<ActionResult> VerifyEmail(VerifyEmailDto model)
		{
			await _accountService.VerifyEmailAsync(model);

			return Ok(new { Message = "Email has been verified successfully." });
		}

		[HttpPost("login")]
		public async Task<ActionResult<LoginResponseDto>> Login(LoginDto model)
		{
			var userDto = await _accountService.LoginAsync(model);
			return Ok(userDto);
		}

		[HttpPost("forgetPassword")]
		public async Task<ActionResult<RegisterResponseDto>> ForgetPassword(ForgetPasswordDto model)
		{
			var dto = await _accountService.ForgetPasswordAsync(model);
			return Ok(dto);
		}

		[HttpPost("verifyForgetPassword")]
		public async Task<ActionResult<RegisterResponseDto>> VerifyForgetPassword(VerifyEmailDto model)
		{
			var user = await _accountService.VerifyForgetPasswordAsync(model.Email, model.Token);
			return Ok(new RegisterResponseDto() { Email = user.Email! });
		}

		[HttpPost("resetPassword")]
		public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
		{
			await _accountService.ResetPasswordAsync(model);
			return Ok(new { message = "Password has been successfully reset." });
		}

	}
}
