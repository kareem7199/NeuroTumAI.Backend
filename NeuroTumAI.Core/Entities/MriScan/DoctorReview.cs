using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.MriScan
{
	public class DoctorReview : BaseEntity
	{
		public string Findings { get; set; }
		public string Reasoning { get; set; }
		public int DoctorId { get; set; }
		public Doctor Doctor { get; set; }
	}
}
