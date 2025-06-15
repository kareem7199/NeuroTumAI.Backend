using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.DoctorSpecs
{
	public class DoctorSpecifications : BaseSpecifications<Doctor>
	{
		public DoctorSpecifications()
			: base()
		{

		}

		public DoctorSpecifications(IEnumerable<int> doctorIds)
			: base(D => doctorIds.Contains(D.Id))
		{
			Includes.Add(D => D.ApplicationUser);
			Includes.Add(D => D.ApplicationUser.DeviceTokens);
		}

		public DoctorSpecifications(string ApplicationUserId)
			: base(D => D.ApplicationUserId == ApplicationUserId && D.IsApproved == true)
		{
			Includes.Add(D => D.ApplicationUser);
		}

		public DoctorSpecifications(int doctorId, bool includeDeviceTokens = false)
			: base(D => D.Id == doctorId)
		{
			Includes.Add(D => D.ApplicationUser);
			Includes.Add(D => D.Reviews);
			if (includeDeviceTokens) Includes.Add(D => D.ApplicationUser.DeviceTokens);
		}
	}
}
