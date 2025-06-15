using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Core.Specifications.ChatMessageSpecs
{
	public class LastMessageSpecifications : BaseSpecifications<ChatMessage>
	{
        public LastMessageSpecifications(int conversationId)
            :base(CM => CM.ConversationId == conversationId)
        {
            AddOrderByDesc(CM => CM.SentAt);
			ApplyPagination(0,1);
		}
	}
}
