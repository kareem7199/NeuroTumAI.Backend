using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Core.Specifications.ClinicSpecs
{
	public class PendingClinicCountSpecifications : BaseSpecifications<Clinic>
	{
		public PendingClinicCountSpecifications(PendingClinicSpecParams specParams)
			: base(
				 C => C.IsApproved == false &&
				 C.Doctor.IsApproved == true &&
				 (string.IsNullOrEmpty(specParams.Search) ||
				 C.Address.ToUpper().Contains(specParams.Search) ||
				 C.Doctor.ApplicationUser.FullName.ToUpper().Contains(specParams.Search) ||
				 C.Doctor.ApplicationUser.NormalizedEmail.Contains(specParams.Search) ||
				 C.Doctor.ApplicationUser.NormalizedUserName.Contains(specParams.Search)))
		{

		}
	}
}
