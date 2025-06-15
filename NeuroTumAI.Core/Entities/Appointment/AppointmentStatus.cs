using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Entities.Appointment
{
	public enum AppointmentStatus
	{
		[EnumMember(Value = "Pending")]
		Pending,
		[EnumMember(Value = "Cancelled")]
		Cancelled,
		[EnumMember(Value = "Completed")]
		Completed
	}
}
