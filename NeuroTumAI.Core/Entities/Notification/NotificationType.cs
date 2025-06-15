using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Entities.Notification
{
	public enum NotificationType
	{
		[Display(Name = "AppointmentCreated")]
		Appointment,

		[Display(Name = "AppointmentCancelled")]
		AppointmentCancellation,

		[Display(Name = "AppointmentReminder")]
		AppointmentReminder,
		[Display(Name = "AppointmentTimeChange")]
		AppointmentTimeChange,
		[Display(Name = "ScanPatient")]
		ScanPatient,

		[Display(Name = "ScanPhysician")]
		ScanPhysician,

		[Display(Name = "SessionStarted")]
		SessionStart,

		[Display(Name = "Message")]
		Message
	}
}
