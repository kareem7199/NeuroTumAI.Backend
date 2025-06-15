using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Clinic;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class RegisterDoctorWithClinicDto
	{
		public DoctorRegisterDto Doctor { get; set; }
        public BaseAddClinicDto Clinic { get; set; }
    }
}
