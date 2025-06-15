using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class ForgetPasswordDto
	{
        [Required(ErrorMessage = "email_required")]
        [EmailAddress(ErrorMessage = "email_invalid")]
        public string Email { get; set; }
    }
}
