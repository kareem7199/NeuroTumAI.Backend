using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Specifications.PostSpecs.SavedPostSpecs
{
	public class SavedPostCursorPaginationSpecifications : BaseSpecifications<SavedPost>
	{
        public SavedPostCursorPaginationSpecifications(string userId , int cursor)
			: base(SP => SP.ApplicationUserId == userId && (cursor < 1 || SP.Id < cursor))
		{
			AddOrderByDesc(P => P.Id);
			Includes.Add(P => P.Post);
			Includes.Add(P => P.Post.Comments);
			Includes.Add(P => P.Post.Likes);
			Includes.Add(P => P.Post.ApplicationUser);
			ApplyPagination(0, 5);
		}
	}
}
