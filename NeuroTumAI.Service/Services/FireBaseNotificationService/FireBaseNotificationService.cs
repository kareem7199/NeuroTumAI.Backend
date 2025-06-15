using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using NeuroTumAI.Core.Entities.Notification;
using NeuroTumAI.Core.Services.Contract;
using Notification = FirebaseAdmin.Messaging.Notification;

namespace NeuroTumAI.Service.Services.FireBaseNotificationService
{
	public class FireBaseNotificationService : IFireBaseNotificationService
	{
		private static bool _isInitialized;
		private readonly ILogger<FireBaseNotificationService> _logger;

		public FireBaseNotificationService(ILogger<FireBaseNotificationService> logger)
		{
			if (!_isInitialized)
			{
				var basePath = AppContext.BaseDirectory;
				var jsonPath = Path.Combine(basePath, "nuerotum-firebase-adminsdk-fbsvc-927b811ac6.json");

				FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.FromFile(jsonPath)
				});
				_isInitialized = true;
			}

			_logger = logger;
		}
		public async Task SendNotificationAsync(string title, string body, string fcmToken, NotificationType notificationType)
		{
			var message = new Message()
			{
				Token = fcmToken,
				Data = new Dictionary<string, string>
				{
					{ "type", notificationType.ToString() }
				},
				Notification = new Notification
				{
					Title = title,
					Body = body,
				}
			};

			try
			{
				await FirebaseMessaging.DefaultInstance.SendAsync(message);
			}
			catch (Exception ex)
			{
				_logger.LogWarning($"can't send notification to {fcmToken} : ${ex.Message}");
			}
		}
	}
}
