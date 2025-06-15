using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Core.Specifications.ConversationSpecs
{
	public class ConversationSpecification : BaseSpecifications<Conversation>
	{
		public ConversationSpecification(string firstUserId, string secondUserId)
			: base(C => C.FirstUserId == firstUserId && C.SecondUserId == secondUserId || C.FirstUserId == secondUserId && C.SecondUserId == firstUserId)
		{
			Includes.Add(C => C.FirstUser);
			Includes.Add(C => C.SecondUser);
		}

		public ConversationSpecification(string userId, PaginationParamsDto specParams)
			: base(C => C.FirstUserId == userId || C.SecondUserId == userId)
		{
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
			AddOrderByDesc(C => C.LastMessageTime);
			Includes.Add(C => C.FirstUser);
			Includes.Add(C => C.SecondUser);
		}
	}
}
