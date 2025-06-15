using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.DoctorSpecs
{
	public class DoctorProfileSpecifications : BaseSpecifications<Doctor>
	{
        public DoctorProfileSpecifications(string ApplicationUserId)
			: base(D => D.ApplicationUserId == ApplicationUserId && D.IsApproved == true)
		{
			Includes.Add(D => D.ApplicationUser);
			Includes.Add(D => D.Reviews);
			Includes.Add(D => D.Clinics);
		}
	}
}
