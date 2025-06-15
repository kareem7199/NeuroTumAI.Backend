using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.MriScan;

namespace NeuroTumAI.Core.Specifications.MriScanSpecs
{
	public class PatientMriScanSpecifications : BaseSpecifications<MriScan>
	{
		public PatientMriScanSpecifications(int patientId, PaginationParamsDto paginationParams)
			: base(MS => MS.PatientId == patientId)
		{
			AddOrderByDesc(MS => MS.UploadDate);
			Includes.Add(MS => MS.DoctorReview);
			Includes.Add(MS => MS.DoctorReview.Doctor.ApplicationUser);
			ApplyPagination((paginationParams.PageIndex - 1) * paginationParams.PageSize, paginationParams.PageSize);
		}

		public PatientMriScanSpecifications(int patientId)
			: base(MS => MS.PatientId == patientId)
		{
		}
	}
}
