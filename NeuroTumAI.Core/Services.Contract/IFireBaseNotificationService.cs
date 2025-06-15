using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Notification;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IFireBaseNotificationService
	{
		Task SendNotificationAsync(string title, string body, string fcmToken, NotificationType notificationType);
	}
}
