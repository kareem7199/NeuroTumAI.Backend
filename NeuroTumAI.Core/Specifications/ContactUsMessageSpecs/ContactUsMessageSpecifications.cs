using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Contact_Us;

namespace NeuroTumAI.Core.Specifications.ContactUsMessageSpecs
{
	public class ContactUsMessageSpecifications : BaseSpecifications<ContactUsMessage>
	{
		public ContactUsMessageSpecifications(PendingMessagesSpecParams specParams)
			: base(M => M.Status == MessageStatus.Pending &&
				(string.IsNullOrEmpty(specParams.Search) ||
				 M.Patient.ApplicationUser.FullName.ToUpper().Contains(specParams.Search) ||
				 M.Patient.ApplicationUser.NormalizedEmail.Contains(specParams.Search) ||
				 M.Patient.ApplicationUser.NormalizedUserName.Contains(specParams.Search)))
		{
			AddOrderBy(M => M.CreatedAt);
			Includes.Add(M => M.Patient);
			Includes.Add(M => M.Patient.ApplicationUser);
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}

		public ContactUsMessageSpecifications(int messageId)
			: base(M => M.Id == messageId && M.Status == MessageStatus.Pending)
		{
			Includes.Add(M => M.Patient);
			Includes.Add(M => M.Patient.ApplicationUser);
		}
	}
}
