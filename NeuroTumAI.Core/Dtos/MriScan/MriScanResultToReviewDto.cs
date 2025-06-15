using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.MriScan;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Dtos.MriScan
{
	public class MriScanResultToReviewDto
	{
		public int Id { get; set; }
		public string ImagePath { get; set; }
		public string DetectionClass { get; set; }
		public string Confidence { get; set; }
		public string AiGeneratedImagePath { get; set; }
		public DateTime UploadDate { get; set; }
		public int PatientId { get; set; }
		public string PatientName { get; set; }
		public string? PatientProfilePicture { get; set; }
		public DateTime PatientDateOfBirth { get; set; }
		public string PatientGender { get; set; }
		public int Age => CalculateAge(PatientDateOfBirth);
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
