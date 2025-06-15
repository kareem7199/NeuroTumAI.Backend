using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Specifications.ReviewSpecs
{
	public class ReviewSpecifications : BaseSpecifications<Review>
	{
        public ReviewSpecifications(int patientId, int doctorId)
            : base(R => R.PatientId == patientId && R.DoctorId == doctorId)
        {
            
        }

        public ReviewSpecifications(int doctorId)
            : base(R => R.DoctorId == doctorId)
        {
            Includes.Add(R => R.Patient);
			Includes.Add(R => R.Patient.ApplicationUser);
			AddOrderByDesc(R => R.CreatedAt);
			ApplyPagination(0, 5);
		}
	}
}
