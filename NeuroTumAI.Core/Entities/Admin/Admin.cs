using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Entities.Admin
{
	public class Admin : BaseEntity
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
	}
}
