using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Post_Aggregate
{
	public class Post : BaseEntity
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
		public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
		public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
		public ICollection<SavedPost> Saves { get; set; } = new HashSet<SavedPost>();
	}
}
