using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Core.Specifications.ClinicSpecs
{
	public class PendingClinicSpecifications : BaseSpecifications<Clinic>
	{
		public PendingClinicSpecifications(PendingClinicSpecParams specParams)
			: base(
				 C => C.IsApproved == false &&
				 C.Doctor.IsApproved == true &&
				 (string.IsNullOrEmpty(specParams.Search) ||
				 C.Address.ToUpper().Contains(specParams.Search) ||
				 C.Doctor.ApplicationUser.FullName.ToUpper().Contains(specParams.Search) ||
				 C.Doctor.ApplicationUser.NormalizedEmail.Contains(specParams.Search) ||
				 C.Doctor.ApplicationUser.NormalizedUserName.Contains(specParams.Search)))
		{
			AddOrderBy(C => C.Id);
			Includes.Add(C => C.Doctor);
			Includes.Add(C => C.Doctor.ApplicationUser);
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}
	}
}
