using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.Chat;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.Chat_Aggregate;
using NeuroTumAI.Core.Services.Contract;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NeuroTumAI.APIs.Controllers.Chat
{
	[Authorize]
	public class ChatController : BaseApiController
	{
		private readonly IChatService _chatService;
		private readonly IMapper _mapper;

		public ChatController(IChatService chatService, IMapper mapper)
		{
			_chatService = chatService;
			_mapper = mapper;
		}

		[HttpPost("sendMessage")]
		public async Task<ActionResult<MessageToReturnDto>> SendMessage([FromBody] SendMessageDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var message = await _chatService.SendMessageAsync(dto, userId);

			return Ok(_mapper.Map<MessageToReturnDto>(message));
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<PaginationDto<ConversationWithLastMessageToReturnDto>>>> GetUserConversations([FromQuery] PaginationParamsDto specParams)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var conversations = await _chatService.GetUserConversationsAsync(userId, specParams);
			var count = await _chatService.GetUserConversationsCountAsync(userId);

			return Ok(new PaginationDto<ConversationWithLastMessageToReturnDto>(specParams.PageIndex, specParams.PageSize, count, conversations));
		}

		[HttpGet("{conversationId}")]
		public async Task<ActionResult<IReadOnlyList<PaginationDto<MessageToReturnDto>>>> GetConversationMessages(int conversationId, [FromQuery] PaginationParamsDto specParams)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var messages = await _chatService.GetConversationMessagesAsync(userId, conversationId, specParams);
			var count = await _chatService.GetConversationMessagesCountAsync(conversationId);

			return Ok(new PaginationDto<MessageToReturnDto>(specParams.PageIndex, specParams.PageSize, count, _mapper.Map<IReadOnlyList<MessageToReturnDto>>(messages)));
		}

		[HttpGet("user/{otherUserId}")]
		public async Task<ActionResult<ConversationDto>> GetConversationByUserId(string otherUserId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var conversation = await _chatService.GetConversationByUserIdAsync(userId, otherUserId);

			return Ok(_mapper.Map<ConversationDto>(conversation));
		}

	}
}
