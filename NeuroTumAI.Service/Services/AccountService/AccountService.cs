using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Resources.Shared;
using NeuroTumAI.Core.Resources.Validation;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.DoctorSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;
using NeuroTumAI.Service.Dtos.Account;
using Newtonsoft.Json.Linq;

namespace NeuroTumAI.Service.Services.AccountService
{
	public class AccountService : IAccountService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailService _emailService;
		private readonly IAuthService _authService;
		private readonly IBlobStorageService _blobStorageService;
		private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ILocalizationService _localizationService;

		public AccountService(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ILocalizationService localizationService,
			IUnitOfWork unitOfWork,
			IEmailService emailService,
			IAuthService authService,
			IBlobStorageService blobStorageService,
			IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_localizationService = localizationService;
			_unitOfWork = unitOfWork;
			_emailService = emailService;
			_authService = authService;
			_blobStorageService = blobStorageService;
			_mapper = mapper;
		}
		public async Task<Patient> RegisterPatientAsync(PatientRegisterDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is not null)
				throw new BadRequestException(_localizationService.GetMessage<ValidationResources>("UniqueEmail"));

			user = await _userManager.FindByNameAsync(model.Username);
			if (user is not null)
				throw new BadRequestException(_localizationService.GetMessage<ValidationResources>("UsernameTaken"));

			string? profilePicture = null;

			if (model.ProfilePicture is not null)
			{
				using var stream = model.ProfilePicture.OpenReadStream();
				var fileUrl = await _blobStorageService.UploadFileAsync(stream, model.ProfilePicture.FileName, "profilepictures");
				profilePicture = fileUrl;
			}

			var newAccount = new ApplicationUser()
			{
				ProfilePicture = profilePicture,
				FullName = model.FullName,
				Email = model.Email,
				UserName = model.Username,
				Gender = (model.Gender == "Male" ? Gender.Male : Gender.Female),
				DateOfBirth = model.DateOfBirth
			};

			var result = await _userManager.CreateAsync(newAccount, model.Password);

			if (!result.Succeeded)
				throw new ValidationException(_localizationService.GetMessage<SharedResources>("ValidationError"))
				{
					Errors = result.Errors.Select((E) => E.Description)
				};

			await _userManager.AddToRoleAsync(newAccount, "Patient");

			var newPatient = new Patient()
			{
				ApplicationUserId = newAccount.Id
			};
			var patientRepo = _unitOfWork.Repository<Patient>();

			patientRepo.Add(newPatient);

			await _unitOfWork.CompleteAsync();

			var token = await _userManager.GenerateUserTokenAsync(newAccount, "EmailConfirmation", "EmailConfirmation");
			SendVerifyEmailMailAsync(model.Email, token);

			return newPatient;

		}

		public async Task<Doctor> RegisterDoctorAsync(RegisterDoctorWithClinicDto model)
		{
			var doctorModel = model.Doctor;
			var clinicModel = model.Clinic;

			var user = await _userManager.FindByEmailAsync(doctorModel.Email);
			if (user is not null)
				throw new BadRequestException(_localizationService.GetMessage<ValidationResources>("UniqueEmail"));

			user = await _userManager.FindByNameAsync(doctorModel.Username);
			if (user is not null)
				throw new BadRequestException(_localizationService.GetMessage<ValidationResources>("UsernameTaken"));

			using var profilePictureStream = doctorModel.ProfilePicture.OpenReadStream();
			var profilePicture = await _blobStorageService.UploadFileAsync(profilePictureStream, doctorModel.ProfilePicture.FileName, "profilepictures");

			using var licenseDocumentFrontStream = doctorModel.LicenseDocumentFront.OpenReadStream();
			var licenseDocumentFront = await _blobStorageService.UploadFileAsync(licenseDocumentFrontStream, doctorModel.LicenseDocumentFront.FileName, "doctor-licenses");


			using var licenseDocumentBackStream = doctorModel.LicenseDocumentBack.OpenReadStream();
			var licenseDocumentBack = await _blobStorageService.UploadFileAsync(licenseDocumentBackStream, doctorModel.LicenseDocumentBack.FileName, "doctor-licenses");

			using var clinicLicenseDocumentStream = clinicModel.LicenseDocument.OpenReadStream();
			var clinicLicenseDocument = await _blobStorageService.UploadFileAsync(clinicLicenseDocumentStream, clinicModel.LicenseDocument.FileName, "clinic-licenses");

			var newAccount = new ApplicationUser()
			{
				ProfilePicture = profilePicture,
				FullName = doctorModel.FullName,
				Email = doctorModel.Email,
				UserName = doctorModel.Username,
				Gender = (doctorModel.Gender == "Male" ? Gender.Male : Gender.Female),
				DateOfBirth = doctorModel.DateOfBirth
			};

			var result = await _userManager.CreateAsync(newAccount, doctorModel.Password);

			if (!result.Succeeded)
				throw new ValidationException(_localizationService.GetMessage<SharedResources>("ValidationError"))
				{
					Errors = result.Errors.Select((E) => E.Description)
				};

			await _userManager.AddToRoleAsync(newAccount, "Doctor");

			var newDoctor = new Doctor()
			{
				ApplicationUserId = newAccount.Id,
				LicenseDocumentBack = licenseDocumentBack,
				LicenseDocumentFront = licenseDocumentFront,
			};

			var location = new NetTopologySuite.Geometries.Point(clinicModel.Longitude, clinicModel.Latitude) { SRID = 4326 };
			var newClinic = new Clinic()
			{
				Address = clinicModel.Address,
				Location = location,
				PhoneNumber = clinicModel.PhoneNumber,
				LicenseDocument = clinicLicenseDocument
			};

			newDoctor.Clinics.Add(newClinic);

			var doctorRepo = _unitOfWork.Repository<Doctor>();

			doctorRepo.Add(newDoctor);

			await _unitOfWork.CompleteAsync();

			var token = await _userManager.GenerateUserTokenAsync(newAccount, "EmailConfirmation", "EmailConfirmation");
			SendVerifyEmailMailAsync(doctorModel.Email, token);

			return newDoctor;
		}

		public async Task<bool> VerifyEmailAsync(VerifyEmailDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("UserNotFound"));

			var isValid = await _userManager.VerifyUserTokenAsync(user, "EmailConfirmation", "EmailConfirmation", model.Token);
			if (!isValid)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("InvalidOrExpiredToken"));

			user.EmailConfirmed = true;
			await _userManager.UpdateAsync(user);

			return true;

		}

		public async Task<LoginResponseDto> LoginAsync(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
				throw new UnAuthorizedException(_localizationService.GetMessage<ResponsesResources>("InvalidCredentials"));

			if (!user.EmailConfirmed)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("InvalidCredentials"));


			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded)
				throw new UnAuthorizedException(_localizationService.GetMessage<ResponsesResources>("InvalidCredentials"));

			var token = await _authService.CreateTokenAsync(user);

			var isPatient = await _userManager.IsInRoleAsync(user, "Patient");

			if (isPatient)
			{
				var patientRepo = _unitOfWork.Repository<Patient>();
				var patientSpec = new PatientSpecifications(user.Id);

				var patient = await patientRepo.GetWithSpecAsync(patientSpec);

				return new LoginResponseDto()
				{
					Token = token,
					User = _mapper.Map<UserDto>(patient)
				};
			}
			else
			{
				var doctorRepo = _unitOfWork.Repository<Doctor>();
				var doctorSpec = new DoctorSpecifications(user.Id);

				var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

				if (doctor is null)
					throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("AccountNotApproved"));

				return new LoginResponseDto()
				{
					Token = token,
					User = _mapper.Map<UserDto>(doctor)
				};
			}
		}

		public async Task<RegisterResponseDto> ForgetPasswordAsync(ForgetPasswordDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("UserNotFound"));
			if (!user.EmailConfirmed)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("UserNotFound"));

			var otpCode = new Random().Next(100000, 999999).ToString();

			await _userManager.RemoveAuthenticationTokenAsync(user, "PasswordReset", "OTP");
			await _userManager.SetAuthenticationTokenAsync(user, "PasswordReset", "OTP", otpCode);

			SendOtpEmailAsync(model.Email, otpCode);

			return new RegisterResponseDto()
			{
				Email = model.Email
			};

		}

		public async Task<ApplicationUser> VerifyForgetPasswordAsync(string email, string otp)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("InvalidOrExpiredToken"));

			var storedOtp = await _userManager.GetAuthenticationTokenAsync(user, "PasswordReset", "OTP");

			if (storedOtp != otp)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("InvalidOrExpiredToken"));

			return user;
		}

		public async Task ResetPasswordAsync(ResetPasswordDto model)
		{
			var user = await VerifyForgetPasswordAsync(model.Email, model.Token);

			var resetResult = await _userManager.RemovePasswordAsync(user);
			if (!resetResult.Succeeded)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("FailedToUpdatePassword"));

			resetResult = await _userManager.AddPasswordAsync(user, model.Password);
			if (!resetResult.Succeeded)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("FailedToUpdatePassword"));

			await _userManager.RemoveAuthenticationTokenAsync(user, "PasswordReset", "OTP");
		}

		private async Task SendVerifyEmailMailAsync(string email, string token)
		{
			string subject = "Email Verification Code";

			string body = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <title>Email Verification</title>
    </head>
    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
        <div style='max-width: 600px; margin: 20px auto; background-color: #ffffff; padding: 20px; border-radius: 8px;
                    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); text-align: center;'>
            
            <h2 style='font-size: 24px; font-weight: bold; color: #333;'>Verify Your Email</h2>
            
            <p style='font-size: 16px; color: #555; margin-bottom: 20px;'>
                Use the following OTP to verify your email address. This OTP is valid for 10 minutes.
            </p>

            <div style='font-size: 32px; font-weight: bold; color: #007bff; margin: 20px 0;'>
                {token}
            </div>

            <p style='font-size: 16px; color: #555;'>
                If you didn't request this, please ignore this email.
            </p>

            <p style='font-size: 14px; color: #999; margin-top: 20px;'>
                Thank you
            </p>

        </div>
    </body>
    </html>
			";

			await _emailService.SendAsync(email, subject, body);
		}

		private async Task SendOtpEmailAsync(string email, string otp)
		{
			string subject = "Your OTP Code";

			var body = $"Your OTP code for password reset is: <b>{otp}</b>. It is valid for 5 minutes.";


			await _emailService.SendAsync(email, subject, body);
		}

		public async Task<UserDto> GetUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user is null)
				throw new UnAuthorizedException("iosdaxhiojfiodsjj");


			var isPatient = await _userManager.IsInRoleAsync(user, "Patient");

			if (isPatient)
			{
				var patientRepo = _unitOfWork.Repository<Patient>();
				var patientSpec = new PatientSpecifications(user.Id);

				var patient = await patientRepo.GetWithSpecAsync(patientSpec);

				return _mapper.Map<UserDto>(patient);
			}
			else
			{
				var doctorRepo = _unitOfWork.Repository<Doctor>();
				var doctorSpec = new DoctorSpecifications(user.Id);

				var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

				return _mapper.Map<UserDto>(doctor);
			}
		}

		public async Task<string> UpdateProfilePictureAsync(string userId, UpdateProfilePictureDto profilePictureDto)
		{
			var user = await _userManager.FindByIdAsync(userId);

			using var stream = profilePictureDto.ProfilePicture.OpenReadStream();
			var fileUrl = await _blobStorageService.UploadFileAsync(stream, profilePictureDto.ProfilePicture.FileName, "profilepictures");
			user!.ProfilePicture = fileUrl;

			await _userManager.UpdateAsync(user);

			return fileUrl;
		}
	}
}
