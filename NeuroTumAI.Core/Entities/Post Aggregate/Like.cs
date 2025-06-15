using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Post_Aggregate
{
	public class Like : BaseEntity
	{
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int PostId { get; set; }
		public Post Post { get; set; }
	}
}
