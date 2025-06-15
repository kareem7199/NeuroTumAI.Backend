using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.UserDeviceTokenSpecs
{
	public class UserDeviceTokenByUserIdSpecifications : BaseSpecifications<UserDeviceToken>
	{
		public UserDeviceTokenByUserIdSpecifications(string userId)
			: base(UDT => UDT.ApplicationUserId == userId)
		{

		}
	}
}
