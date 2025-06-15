using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Specifications.ReviewSpecs
{
	public class DoctorReviewSpecifications : BaseSpecifications<Review>
	{
		public DoctorReviewSpecifications(int doctorId, PaginationParamsDto specParams)
			: base(R => R.DoctorId == doctorId)
		{
			Includes.Add(R => R.Patient);
			Includes.Add(R => R.Patient.ApplicationUser);
			AddOrderByDesc(R => R.CreatedAt);
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}

		public DoctorReviewSpecifications(int doctorId)
			: base(R => R.DoctorId == doctorId)
		{
		}
	}
}
