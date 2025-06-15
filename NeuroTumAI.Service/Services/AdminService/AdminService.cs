using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Entities.Admin;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AdminSpecs;

namespace NeuroTumAI.Service.Services.AdminService
{
	public class AdminService : IAdminService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuthService _authService;

		public AdminService(IUnitOfWork unitOfWork, IAuthService authService)
        {
			_unitOfWork = unitOfWork;
			_authService = authService;
		}

		public async Task<Admin> GetAdminByIdAsync(int id)
		{
			var adminRepo = _unitOfWork.Repository<Admin>();
			return await adminRepo.GetAsync(id);
		}

		public async Task<string> LoginAdminAsync(LoginDto model)
		{
			var adminRepo = _unitOfWork.Repository<Admin>();
			var adminSpec = new AdminSpecifications(model.Email);
			var admin = await adminRepo.GetWithSpecAsync(adminSpec);

			if (admin is null || !BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
				throw new UnAuthorizedException("Invalid email or password.");

			var token = await _authService.CreateTokenAsync(admin);

			return token;
		}
	}
}
