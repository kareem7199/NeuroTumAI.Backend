using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Notification
{
	public class AppointmentTimeChangeNotificationDto
	{
		public int PatientId { get; set; }
		public DateOnly Date { get; set; }
        public TimeOnly OldTime { get; set; }
    }
}
