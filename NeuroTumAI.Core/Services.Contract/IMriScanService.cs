using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Dtos.MriScan;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.MriScan;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IMriScanService
	{
		Task<MriScan> UploadAndProcessMriScanAsync(PredictRequestDto model, string userId);
		Task<IReadOnlyList<MriScan>> GetExpiredUnreviewedScansAsync();
		Task<IReadOnlyList<DoctorMriAssignment>> GetAssignedScansAsync(string userId, PaginationParamsDto dto);
		Task<int> GetAssignedScansCountAsync(string userId);
		Task ReviewAsync(int mriScanId, string userId, AddMriScanReviewDto scanReviewDto);
		Task<IReadOnlyList<MriScan>> GetPatientScansAsync(string userId, PaginationParamsDto dto);
		Task<int> GetPatientScansCountAsync(string userId);
		Task AutoReviewAsync(int mriId);
	}
}
