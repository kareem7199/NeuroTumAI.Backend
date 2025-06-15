using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Notification;

namespace NeuroTumAI.Core.Specifications.NotificationSpecs
{
	public class NotificationSpecifications : BaseSpecifications<Notification>
	{
		public NotificationSpecifications(string userId, NotificationSpecParams specParams)
			: base(N => N.ApplicationUserId == userId)
		{
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}

		public NotificationSpecifications(string userId)
			: base(N => N.ApplicationUserId == userId)
		{
		}
	}
}
