using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Identity
{
	public enum Gender
	{
		[EnumMember(Value = "Male")]
		Male,
		[EnumMember(Value = "Female")]
		Female
	}
}
