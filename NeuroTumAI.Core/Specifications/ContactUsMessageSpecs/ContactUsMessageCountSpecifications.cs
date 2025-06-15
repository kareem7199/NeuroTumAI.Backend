using NeuroTumAI.Core.Entities.Contact_Us;

namespace NeuroTumAI.Core.Specifications.ContactUsMessageSpecs
{
	public class ContactUsMessageCountSpecifications : BaseSpecifications<ContactUsMessage>
	{
		public ContactUsMessageCountSpecifications(PendingMessagesSpecParams specParams)
			: base(M => M.Status == MessageStatus.Pending &&
				(string.IsNullOrEmpty(specParams.Search) ||
				 M.Patient.ApplicationUser.FullName.ToUpper().Contains(specParams.Search) ||
				 M.Patient.ApplicationUser.NormalizedEmail.Contains(specParams.Search) ||
				 M.Patient.ApplicationUser.NormalizedUserName.Contains(specParams.Search)))
		{

		}
	}
}
