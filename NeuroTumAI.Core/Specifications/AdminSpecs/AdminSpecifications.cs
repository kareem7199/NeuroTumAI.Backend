using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Admin;

namespace NeuroTumAI.Core.Specifications.AdminSpecs
{
	public class AdminSpecifications : BaseSpecifications<Admin>
	{
        public AdminSpecifications(string email)
            :base(A => A.Email == email)
        {
            
        }
    }
}
