using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.MriScan
{
	public class DoctorReviewDto
	{
        public string Findings { get; set; }
		public string Reasoning { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorProfilePicture { get; set; }
    }
}
