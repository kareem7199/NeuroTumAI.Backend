using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Chat_Aggregate
{
	public class Conversation : BaseEntity
	{
		public string FirstUserId { get; set; }
		public string SecondUserId { get; set; }
		public DateTime LastMessageTime { get; set; } = DateTime.UtcNow;
		public ApplicationUser FirstUser { get; set; }
		public ApplicationUser SecondUser { get; set; }
		public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
	}
}
