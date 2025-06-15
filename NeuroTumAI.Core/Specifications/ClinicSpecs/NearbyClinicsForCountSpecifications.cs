using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Core.Specifications.ClinicSpecs
{
	public class NearbyClinicsForCountSpecifications : BaseSpecifications<Clinic>
	{
		public NearbyClinicsForCountSpecifications(ClinicSpecParams specParams)
			: base(C => C.IsApproved == true && (string.IsNullOrEmpty(specParams.Search) || C.Doctor.ApplicationUser.FullName.ToLower().Contains(specParams.Search)))
		{

		}
	}
}
