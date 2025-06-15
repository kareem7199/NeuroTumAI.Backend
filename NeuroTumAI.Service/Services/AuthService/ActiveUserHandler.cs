using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Authorization;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Specifications.DoctorSpecs;

namespace NeuroTumAI.Service.Services.AuthService
{
	public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;

		public ActiveUserHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_unitOfWork = unitOfWork;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
		{
			var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var user = await _userManager.FindByIdAsync(userId);
			if (user is null)
			{
				context.Fail();
				return;
			}

			var isDoctor = await _userManager.IsInRoleAsync(user, "Doctor");

			if (isDoctor)
			{
				var doctorRepo = _unitOfWork.Repository<Doctor>();

				var doctorSpec = new DoctorSpecifications(userId);
				var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

				if (doctor is null || !doctor.IsApproved)
				{
					context.Fail();
					return;
				}
			}

			context.Succeed(requirement);

		}
	}
}
