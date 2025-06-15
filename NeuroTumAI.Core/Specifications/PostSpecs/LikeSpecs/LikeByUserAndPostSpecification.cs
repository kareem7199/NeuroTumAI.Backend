using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Specifications.PostSpecs.LikeSpecs
{
	public class LikeByUserAndPostSpecification : BaseSpecifications<Like>
	{
		public LikeByUserAndPostSpecification(string userId, int postId)
			: base(L => L.PostId == postId && L.ApplicationUserId == userId)
		{

		}

		public LikeByUserAndPostSpecification(string userId, List<int> postIds)
			: base(L => L.ApplicationUserId == userId && postIds.Contains(L.PostId))
		{

		}
	}
}
