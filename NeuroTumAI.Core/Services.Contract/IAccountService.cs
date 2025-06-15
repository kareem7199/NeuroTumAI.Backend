using Microsoft.AspNetCore.Identity;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IAccountService
	{
		Task<UserDto> GetUserAsync(string userId);
		Task<Patient> RegisterPatientAsync(PatientRegisterDto model);
		Task<Doctor> RegisterDoctorAsync(RegisterDoctorWithClinicDto model);
		Task<bool> VerifyEmailAsync(VerifyEmailDto model);
		Task<LoginResponseDto> LoginAsync(LoginDto model);
		Task<string> UpdateProfilePictureAsync(string userId, UpdateProfilePictureDto profilePictureDto);
		Task<RegisterResponseDto> ForgetPasswordAsync(ForgetPasswordDto model);
		Task<ApplicationUser> VerifyForgetPasswordAsync(string email, string otp);
		Task ResetPasswordAsync(ResetPasswordDto model);
	}
}
