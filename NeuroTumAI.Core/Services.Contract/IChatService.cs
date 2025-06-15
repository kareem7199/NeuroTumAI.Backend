using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Chat;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IChatService
	{
		Task<ChatMessage> SendMessageAsync(SendMessageDto sendMessageDto, string userId);
		Task<IReadOnlyList<ConversationWithLastMessageToReturnDto>> GetUserConversationsAsync(string userId, PaginationParamsDto model);
		Task<int> GetUserConversationsCountAsync(string userId);
		Task<IReadOnlyList<ChatMessage>> GetConversationMessagesAsync(string userId, int conversationId , PaginationParamsDto model);
		Task<int> GetConversationMessagesCountAsync(int conversationId);
		Task<Conversation> GetConversationByUserIdAsync(string userId, string otherUserId);

	}
}
