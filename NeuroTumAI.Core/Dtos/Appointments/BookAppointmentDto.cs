using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Appointments
{
	public class BookAppointmentDto
	{
		[Required(ErrorMessage = "SlotIdRequired")]
		public int SlotId { get; set; }

		[Required(ErrorMessage = "DateRequired")]
		public DateOnly Date { get; set; }
	}
}
