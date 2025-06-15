using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IUserDeviceTokenService
	{
		Task AddDeviceTokenAsync(string userId, string deviceToken);
		Task RemoveDeviceTokenAsync(string userId, string deviceToken);
	}
}
