using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.UserDeviceTokenSpecs
{
	public class UserDeviceTokenSpecifications : BaseSpecifications<UserDeviceToken>
	{
		public UserDeviceTokenSpecifications(string deviceToken)
			: base(UDT => UDT.FcmToken == deviceToken)
		{

		}
	}
}
