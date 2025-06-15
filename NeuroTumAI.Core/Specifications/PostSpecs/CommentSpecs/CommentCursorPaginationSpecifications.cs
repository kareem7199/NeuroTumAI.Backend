using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Specifications.PostSpecs.CommentSpecs
{
	public class CommentCursorPaginationSpecifications : BaseSpecifications<Comment>
	{
        public CommentCursorPaginationSpecifications(int cursor, int postId)
			: base(P => P.PostId == postId &&  (cursor < 1 || P.Id < cursor))
		{
			AddOrderByDesc(P => P.Id);
			Includes.Add(P => P.ApplicationUser);
			ApplyPagination(0, 5);
		}
	}
}
