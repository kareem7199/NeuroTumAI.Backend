using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Specifications.PostSpecs
{
	public class PostCursorPaginationSpecifications : BaseSpecifications<Post>
	{
        public PostCursorPaginationSpecifications(int cursor)
            :base(P => cursor < 1 || P.Id < cursor)
        {
            AddOrderByDesc(P => P.Id);
			Includes.Add(P => P.Comments);
			Includes.Add(P => P.Likes);
			Includes.Add(P => P.ApplicationUser);
			ApplyPagination(0, 5);
		}
	}
}
