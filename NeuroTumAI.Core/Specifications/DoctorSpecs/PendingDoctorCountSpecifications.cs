using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.DoctorSpecs
{
	public class PendingDoctorCountSpecifications : BaseSpecifications<Doctor>
	{
		public PendingDoctorCountSpecifications(PendingDoctorSpecParams specParams)
			: base(D => !D.IsApproved && (string.IsNullOrEmpty(specParams.Search) || D.ApplicationUser.FullName.ToUpper().Contains(specParams.Search) || D.ApplicationUser.NormalizedEmail.Contains(specParams.Search) || D.ApplicationUser.NormalizedUserName.Contains(specParams.Search)))
		{

		}
	}
}
