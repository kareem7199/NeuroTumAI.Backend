using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class DoctorRegisterDto : RegisterDto
	{
		[Required(ErrorMessage = "ProfilePictureRequired")]
		public IFormFile ProfilePicture { get; set; }
		[Required(ErrorMessage = "LicenseDocumentFrontRequired")]
		public IFormFile LicenseDocumentFront { get; set; }

		[Required(ErrorMessage = "LicenseDocumentBackRequired")]
		public IFormFile LicenseDocumentBack { get; set; }
	}
}
