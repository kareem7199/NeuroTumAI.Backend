using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.MriScan;

namespace NeuroTumAI.Core.Specifications.DoctorMriAssignmentsSpecs
{
	public class DoctorMriAssignmentsSpecifications : BaseSpecifications<DoctorMriAssignment>
	{
		public DoctorMriAssignmentsSpecifications(int doctorId, PaginationParamsDto paginationParams)
			: base(DMA => DMA.DoctorId == doctorId)
		{
			Includes.Add(DMA => DMA.MriScan);
			Includes.Add(DMA => DMA.MriScan.Patient);
			Includes.Add(DMA => DMA.MriScan.Patient.ApplicationUser);
			ApplyPagination((paginationParams.PageIndex - 1) * paginationParams.PageSize, paginationParams.PageSize);
		}

		public DoctorMriAssignmentsSpecifications(int doctorId)
			: base(DMA => DMA.DoctorId == doctorId)
		{
		}
	}
}
