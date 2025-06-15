using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class CommentToReturnDto
	{
        public int Id { get; set; }
        public string Text { get; set; }
		public DateTime CreatedAt { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
        public string UserProfilePicture { get; set; }
    }
}
