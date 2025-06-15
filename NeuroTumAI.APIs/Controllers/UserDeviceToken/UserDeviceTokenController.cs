using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.UserDeviceToken;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.UserDeviceToken
{
	public class UserDeviceTokenController : BaseApiController
	{
		private readonly IUserDeviceTokenService _userDeviceTokenService;

		public UserDeviceTokenController(IUserDeviceTokenService userDeviceTokenService)
		{
			_userDeviceTokenService = userDeviceTokenService;
		}

		[Authorize]
		[HttpPost]
		public async Task<ActionResult> AddDeviceToken(DeviceTokenRequest model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			await _userDeviceTokenService.AddDeviceTokenAsync(userId, model.DeviceToken);

			return Ok();
		}

		[HttpDelete("{userId}")]
		public async Task<ActionResult> DeleteDeviceToken(string userId , [FromQuery] string deviceToken)
		{
			await _userDeviceTokenService.RemoveDeviceTokenAsync(userId, deviceToken);

			return Ok();
		}
	}
}
