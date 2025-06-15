using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Clinic
{
	public class SlotToReturnDto
	{
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
		public TimeOnly StartTime { get; set; }
		public bool IsAvailable { get; set; }
	}
}
