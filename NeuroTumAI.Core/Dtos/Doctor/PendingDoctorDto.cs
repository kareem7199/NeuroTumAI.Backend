using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Review;

namespace NeuroTumAI.Core.Dtos.Doctor
{
	public class PendingDoctorDto
	{
		public int Id { get; set; }
		public string ProfilePicture { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
		public int Age => CalculateAge(DateOfBirth);
		public string LicenseDocumentFront { get; set; }
		public string LicenseDocumentBack { get; set; }
		private int CalculateAge(DateTime dateOfBirth)
		{
			var today = DateTime.Today;
			var age = today.Year - dateOfBirth.Year;

			if (dateOfBirth.Date > today.AddYears(-age))
				age--;

			return age;
		}
	}
}
