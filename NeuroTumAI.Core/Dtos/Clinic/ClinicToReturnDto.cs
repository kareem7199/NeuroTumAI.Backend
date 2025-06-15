using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Dtos.Clinic
{
	public class ClinicToReturnDto
	{
        public int Id { get; set; }
        public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string LicenseDocument { get; set; }
		public bool IsApproved { get; set; } = false;
	}
}
