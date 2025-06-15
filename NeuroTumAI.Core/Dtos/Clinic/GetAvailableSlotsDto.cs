using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Clinic
{
	public class GetAvailableSlotsDto
	{
		[Required(ErrorMessage = "DateRequired")]
		public DateOnly Date { get; set; }
	}
}
