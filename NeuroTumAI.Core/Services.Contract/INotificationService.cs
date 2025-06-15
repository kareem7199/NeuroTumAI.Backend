using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Entities.Notification;
using NeuroTumAI.Core.Specifications.NotificationSpecs;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface INotificationService
	{
		Task SendAppointmentCancellationNotificationsAsync(List<AppointmentCancellationNotificationDto> notifications);
		Task SendAppointmentTimeChangeNotificationsAsync(List<AppointmentTimeChangeNotificationDto> notifications, TimeOnly time);
		Task SendMriScanAssignmentNotificationToDoctorAsync(List<int> notifications);
		Task<IReadOnlyList<Notification>> GetNotificationsAsync(string userId, NotificationSpecParams specParams);
		Task<int> GetNotificationsCountAsync(string userId);
		Task SendNewAppointmentNotificationAsync(NewAppointmentNotificationDto notification);
		Task SendReadyMriScanNotificationAsync(int patientId);
		Task SendMessageNotificationAsync(string userId, string message, string userName);
	}
}
