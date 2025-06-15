using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Specifications.PostSpecs
{
	public class PostSpecifications : BaseSpecifications<Post>
	{
        public PostSpecifications(int postId)
            :base(P => P.Id == postId)
        {
            Includes.Add(P => P.Comments);
            Includes.Add(P => P.Likes);
            Includes.Add(P => P.Saves);
		}
	}
}
