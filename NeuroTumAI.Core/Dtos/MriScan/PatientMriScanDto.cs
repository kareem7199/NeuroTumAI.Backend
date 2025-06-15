using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.MriScan
{
	public class PatientMriScanDto
	{
        public string ImagePath { get; set; }
        public string DetectionClass { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime UploadDate { get; set; }
        public DoctorReviewDto DoctorReview { get; set; }
    }
}
