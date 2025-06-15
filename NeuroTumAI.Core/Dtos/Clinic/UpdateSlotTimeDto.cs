using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Clinic
{
	public class UpdateSlotTimeDto
	{
		[Required(ErrorMessage = "StartTimeRequired")]
		[Range(typeof(TimeOnly), "00:00", "23:59", ErrorMessage = "StartTimeRange")]
		public TimeOnly StartTime { get; set; }
	}
}
