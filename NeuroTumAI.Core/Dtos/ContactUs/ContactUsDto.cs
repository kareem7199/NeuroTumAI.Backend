using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.ContactUs
{
	public class ContactUsDto
	{

		[Required(ErrorMessage = "RequiredMessage")]
		[StringLength(1000, ErrorMessage = "MessageTooLong")]
		public string Message { get; set; }
	}
}
