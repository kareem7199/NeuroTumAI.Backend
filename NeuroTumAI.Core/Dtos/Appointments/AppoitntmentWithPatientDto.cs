using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Account;

namespace NeuroTumAI.Core.Dtos.Appointments
{
	public class AppoitntmentWithPatientDto : AppointmentToReturnDto
	{
        public PublicPatientDto Patient { get; set; }
    }
}
