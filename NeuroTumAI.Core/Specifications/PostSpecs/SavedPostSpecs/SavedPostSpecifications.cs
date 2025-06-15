using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Specifications.PostSpecs.SavedPostSpecs
{
	public class SavedPostSpecifications : BaseSpecifications<SavedPost>
	{
        public SavedPostSpecifications(string userId , int postId)
            :base(SP => SP.PostId == postId && SP.ApplicationUserId == userId)
        {    
        }

        public SavedPostSpecifications(string userId, List<int> postIds)
			: base(SP => SP.ApplicationUserId == userId && postIds.Contains(SP.PostId))
		{

		}
	}
}
