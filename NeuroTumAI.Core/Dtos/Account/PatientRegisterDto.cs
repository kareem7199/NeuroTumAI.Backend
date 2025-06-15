using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NeuroTumAI.Core.Dtos.Account;

namespace NeuroTumAI.Service.Dtos.Account
{
	public class PatientRegisterDto : RegisterDto
	{
		public IFormFile? ProfilePicture { get; set; }
	}
}
