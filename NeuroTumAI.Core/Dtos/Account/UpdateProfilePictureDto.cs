using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class UpdateProfilePictureDto
	{
		[Required(ErrorMessage = "ProfilePictureRequired")]
		public IFormFile ProfilePicture { get; set; }
	}
}
