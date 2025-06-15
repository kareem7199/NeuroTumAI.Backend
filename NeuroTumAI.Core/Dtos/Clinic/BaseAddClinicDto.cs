using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NeuroTumAI.Core.Dtos.Clinic
{
	public class BaseAddClinicDto
	{
		[Required(ErrorMessage = "AddressRequired")]
		public string Address { get; set; }

		[Required(ErrorMessage = "PhoneNumberRequired")]
		public string PhoneNumber { get; set; }
		
		[Required(ErrorMessage = "LicenseDocumentRequired")]
		public IFormFile LicenseDocument { get; set; }

		[Range(-90, 90, ErrorMessage = "latitude_range")]
		public double Latitude { get; set; }

		[Range(-180, 180, ErrorMessage = "longitude_range")]
		public double Longitude { get; set; }
	}
}
