using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Clinic
{
	public class PendingClinicDto
	{
		public int Id { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string LicenseDocument { get; set; }
		public string DoctorName { get; set; }
        public string DoctorProfilePicture { get; set; }
    }
}
