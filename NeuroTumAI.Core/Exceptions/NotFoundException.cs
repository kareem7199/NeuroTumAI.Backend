using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Exceptions
{
	public class NotFoundException : ApplicationException
	{
		public NotFoundException(string message) :
			base(message)
		{
		}
	}
}
