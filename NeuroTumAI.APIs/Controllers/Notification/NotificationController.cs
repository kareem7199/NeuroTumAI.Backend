using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.NotificationSpecs;
using AutoMapper;
using System.Security.Claims;

namespace NeuroTumAI.APIs.Controllers.Notification
{
	public class NotificationController : BaseApiController
	{
		private readonly INotificationService _notificationService;
		private readonly IMapper _mapper;
		private readonly ILocalizationService _localizationService;

		public NotificationController(INotificationService notificationService, IMapper mapper, ILocalizationService localizationService)
		{
			_notificationService = notificationService;
			_mapper = mapper;
			_localizationService = localizationService;
		}

		[HttpGet]
		public async Task<ActionResult<PaginationDto<NotificationToReturnDto>>> List([FromQuery] NotificationSpecParams specParams)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var notifications = await _notificationService.GetNotificationsAsync(userId, specParams);
			var count = await _notificationService.GetNotificationsCountAsync(userId);

			var userLanguage = _localizationService.GetCurrentLanguage();

			var data = _mapper.Map<IReadOnlyList<NotificationToReturnDto>>(notifications, opt => opt.Items["Language"] = userLanguage);

			return Ok(new PaginationDto<NotificationToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}
	}
}
