using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Notification
{
	public class Notification : BaseEntity
	{
		public NotificationType Type { get; set; }
		public string TitleAR { get; set; }
		public string TitleEN { get; set; }
		public string BodyAR { get; set; }
		public string BodyEN { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
	}
}
