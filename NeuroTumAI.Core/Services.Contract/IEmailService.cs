using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IEmailService
	{
		Task SendAsync(string recipients, string subject, string body);
	}
}
