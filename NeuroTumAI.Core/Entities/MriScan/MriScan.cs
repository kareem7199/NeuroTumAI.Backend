using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.MriScan
{
	public class MriScan : BaseEntity
	{
		public string ImagePath { get; set; }
		public string DetectionClass { get; set; }
		public string Confidence { get; set; }
		public string AiGeneratedImagePath { get; set; }
		public bool IsReviewed { get; set; } = false;
		public DateTime UploadDate { get; set; } = DateTime.UtcNow;
		public int PatientId { get; set; }
		public int? DoctorReviewId { get; set; }
		public Patient Patient { get; set; }
		public DoctorReview? DoctorReview { get; set; }
		public ICollection<DoctorMriAssignment> DoctorAssignments { get; set; } = new HashSet<DoctorMriAssignment>();

	}
}
