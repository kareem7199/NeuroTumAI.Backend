using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Core.Specifications.ClinicSpecs
{
	public class ClinicSpecifications : BaseSpecifications<Clinic>
	{
        public ClinicSpecifications()
            : base()
        {
            
        }
        public ClinicSpecifications(int doctorId) : base(C => C.DoctorId == doctorId)
        {
            
        }
    }
}
