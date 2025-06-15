using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Core.Specifications.ChatMessageSpecs
{
	public class ChatMessageSpecifications : BaseSpecifications<ChatMessage>
	{
		public ChatMessageSpecifications(int conversationId, PaginationParamsDto specParams)
			: base(CM => CM.ConversationId == conversationId)
		{
			AddOrderByDesc(CM => CM.SentAt);
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}

		public ChatMessageSpecifications(int conversationId)
			: base(CM => CM.ConversationId == conversationId)
		{
		}
	}
}
