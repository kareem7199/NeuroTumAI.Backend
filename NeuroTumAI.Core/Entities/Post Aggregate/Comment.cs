using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Post_Aggregate
{
	public class Comment : BaseEntity
	{
		public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
	}
}
