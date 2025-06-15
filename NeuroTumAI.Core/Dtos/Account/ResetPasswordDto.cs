using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class ResetPasswordDto
	{
		[Required(ErrorMessage = "email_required")]
		[EmailAddress(ErrorMessage = "email_invalid")]
		public string Email { get; set; }

		[Required(ErrorMessage = "TokenRequired")]
		[RegularExpression(@"^\d{6}$", ErrorMessage = "TokenInvalid")]
		public string Token { get; set; }
		[Required(ErrorMessage = "password_required")]
		[MinLength(6, ErrorMessage = "password_minLength")]
		[RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-]).{6,}$",
	ErrorMessage = "password_complexity")]
		public string Password { get; set; } = string.Empty;
	}
}
