using NeuroTumAI.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Entities.Contact_Us
{
	public class ContactUsMessage : BaseEntity
	{
		public int PatientId { get; set; }
		public Patient Patient { get; set; }
		public string Message { get; set; }
		public MessageStatus Status { get; set; } = MessageStatus.Pending;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	}
}
