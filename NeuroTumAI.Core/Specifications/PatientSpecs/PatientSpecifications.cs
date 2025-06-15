using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.PatientSpecs
{
	public class PatientSpecifications : BaseSpecifications<Patient>
	{
		public PatientSpecifications(string ApplicationUserId)
			: base(P => P.ApplicationUserId == ApplicationUserId)
		{
			Includes.Add(P => P.ApplicationUser);
		}

		public PatientSpecifications(IEnumerable<int> patientIds)
			: base(P => patientIds.Contains(P.Id))
		{
			Includes.Add(P => P.ApplicationUser);
			Includes.Add(P => P.ApplicationUser.DeviceTokens);
		}

		public PatientSpecifications(int patientId)
			: base(P => patientId == P.Id)
		{
			Includes.Add(P => P.ApplicationUser);
			Includes.Add(P => P.ApplicationUser.DeviceTokens);
		}
	}
}
