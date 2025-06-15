using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Exceptions
{
	public class UnAuthorizedException : Exception
	{
		public UnAuthorizedException(string message) : base(message) { }


	}
}
