using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Entities.Clinic_Aggregate
{
	public class Slot : BaseEntity
	{
		public DayOfWeek DayOfWeek { get; set; }
		public TimeOnly StartTime { get; set; }
		public bool IsAvailable { get; set; } = true;
		public int ClinicId { get; set; }
		public Clinic Clinic { get; set; }
	}
}
