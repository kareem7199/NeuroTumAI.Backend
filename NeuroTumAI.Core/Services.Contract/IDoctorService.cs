using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Specifications.DoctorSpecs;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IDoctorService
	{
		Task<Doctor> GetDoctorByClinicIdAsync(int clinicId);
		Task<IReadOnlyList<Doctor>> GetPendingDoctorsAsync(PendingDoctorSpecParams model);
		Task<Doctor> AcceptPendingDoctorAsync(int doctorId);
		Task<Doctor> GetDoctorProfileAsync(string userId);
		Task RejectPendingDoctorAsync(int doctorId);
		Task<int> GetPendingDoctorsCountAsync(PendingDoctorSpecParams model);
		Task<Doctor> GetDoctorByUserIdAsync(string userId);
		Task<Doctor> GetDoctorByIdAsync(int doctorId);
	}
}
