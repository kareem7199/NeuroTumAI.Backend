using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.AccountController
{
	[Authorize(Policy = "ActiveUserOnly")]
	public class AccountController : BaseApiController
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public async Task<ActionResult<UserDto>> GetUserData()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var userDto = await _accountService.GetUserAsync(userId);
			return Ok(userDto);
		}

		[HttpPut("profilePicture")]
		public async Task<ActionResult> UpdateProfilePicture([FromForm] UpdateProfilePictureDto profilePictureDto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var profilePicture = await _accountService.UpdateProfilePictureAsync(userId, profilePictureDto);
			
			return Ok(new
			{
				ProfilePicture = profilePicture
			});
		}
	}
}
