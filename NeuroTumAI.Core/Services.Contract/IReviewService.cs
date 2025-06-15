using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IReviewService
	{
		Task<IReadOnlyList<Review>> GetDoctorLatest5ReviewsAsync(int doctorId);
		Task<Review> AddReviewAsync(AddReviewDto addReviewDto, string userId);
		Task<IReadOnlyList<Review>> GetDoctorReviewsAsync(int doctorId, PaginationParamsDto specParams);
		Task<int> GetDoctorReviewsCountAsync(int doctorId);

	}
}
