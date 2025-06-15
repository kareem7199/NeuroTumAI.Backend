using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class PostToReturnDto
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsLiked { get; set; }
        public bool IsSaved { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
	}
}
