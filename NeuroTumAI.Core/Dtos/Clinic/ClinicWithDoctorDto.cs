using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Clinic
{
	public class ClinicWithDoctorDataDto
	{
		public int Id { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
        public string DoctorProfilePicture { get; set; }
		public double AverageStarRating { get; set; }
		public string DoctorFullName { get; set; }
    }
}
