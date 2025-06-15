using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class LoginDto
	{
		[Required(ErrorMessage = "email_required")]
		[EmailAddress(ErrorMessage = "email_invalid")]
		public string Email { get; set; }

		[Required(ErrorMessage = "password_required")]
		public string Password { get; set; }
	}
}
