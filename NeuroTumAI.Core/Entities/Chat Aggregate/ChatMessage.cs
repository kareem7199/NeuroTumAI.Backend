using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Chat_Aggregate
{
	public class ChatMessage : BaseEntity
	{
		public string SenderId { get; set; }
		public string Content { get; set; }
		public int ConversationId { get; set; }
		public DateTime SentAt { get; set; } = DateTime.UtcNow;
		public Conversation Conversation { get; set; }
		public ApplicationUser Sender { get; set; }
	}
}
