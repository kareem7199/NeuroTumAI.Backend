using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Chat;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Entities.Chat_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications;
using NeuroTumAI.Core.Specifications.ChatMessageSpecs;
using NeuroTumAI.Core.Specifications.ConversationSpecs;
using NeuroTumAI.Service.Hubs;

namespace NeuroTumAI.Service.Services.ChatService
{
	public class ChatService : IChatService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHubContext<ChatHub> _chatHubContext;
		private readonly IMapper _mapper;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly INotificationService _notificationService;

		public ChatService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHubContext<ChatHub> chatHubContext, IMapper mapper, RoleManager<IdentityRole> roleManager, INotificationService notificationService)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_chatHubContext = chatHubContext;
			_mapper = mapper;
			_roleManager = roleManager;
			_notificationService = notificationService;
		}

		private async Task DeliverMessageAsync(string receiverId, MessageToReturnDto message)
		{
			await _chatHubContext.Clients.User(receiverId).SendAsync("ReceiveMessage", message);
		}

		public async Task<ChatMessage> SendMessageAsync(SendMessageDto sendMessageDto, string userId)
		{
			if (userId == sendMessageDto.ReceiverId)
				throw new BadRequestException("The sender and receiver cannot be the same user.");

			var conversationRepo = _unitOfWork.Repository<Conversation>();
			var conversationSpec = new ConversationSpecification(userId, sendMessageDto.ReceiverId);
			var conversation = await conversationRepo.GetWithSpecAsync(conversationSpec);

			if (conversation is null)
			{
				var receiver = await _userManager.FindByIdAsync(sendMessageDto.ReceiverId);
				if (receiver is null)
					throw new NotFoundException("Receiver with the specified ID not found.");

				conversation = new Conversation()
				{
					FirstUserId = userId,
					SecondUserId = sendMessageDto.ReceiverId
				};

				conversationRepo.Add(conversation);
				await _unitOfWork.CompleteAsync();
			}

			var newMessage = new ChatMessage()
			{
				Content = sendMessageDto.Content,
				ConversationId = conversation.Id,
				SenderId = userId
			};

			var chatMessageRepo = _unitOfWork.Repository<ChatMessage>();

			conversation.LastMessageTime = DateTime.UtcNow;

			chatMessageRepo.Add(newMessage);

			await _unitOfWork.CompleteAsync();

			await DeliverMessageAsync(sendMessageDto.ReceiverId, _mapper.Map<MessageToReturnDto>(newMessage));

			var sender = await _userManager.FindByIdAsync(userId);

			await _notificationService.SendMessageNotificationAsync(sendMessageDto.ReceiverId, sendMessageDto.Content, sender!.FullName);

			return newMessage;
		}

		public async Task<IReadOnlyList<ConversationWithLastMessageToReturnDto>> GetUserConversationsAsync(string userId, PaginationParamsDto model)
		{
			var conversationRepo = _unitOfWork.Repository<Conversation>();
			var conversationSpecs = new ConversationSpecification(userId, model);
			var conversations = await conversationRepo.GetAllWithSpecAsync(conversationSpecs);

			var ChatMessageRepo = _unitOfWork.Repository<ChatMessage>();

			foreach (var conversation in conversations)
			{
				if (conversation.FirstUserId == userId)
					conversation.FirstUser = conversation.SecondUser;

				var lastMessageSpecs = new LastMessageSpecifications(conversation.Id);
				var lastMessages = await ChatMessageRepo.GetAllWithSpecAsync(lastMessageSpecs);

				conversation.ChatMessages.Add(lastMessages[0]);
			}

			var conversationsData = _mapper.Map<IReadOnlyList<ConversationWithLastMessageToReturnDto>>(conversations);

			return conversationsData;
		}

		public async Task<int> GetUserConversationsCountAsync(string userId)
		{
			var conversationRepo = _unitOfWork.Repository<Conversation>();
			var conversationSpecs = new UserConversationSpecification(userId);
			return await conversationRepo.GetCountAsync(conversationSpecs);
		}

		public async Task<IReadOnlyList<ChatMessage>> GetConversationMessagesAsync(string userId, int conversationId, PaginationParamsDto model)
		{
			var conversationRepo = _unitOfWork.Repository<Conversation>();
			var conversation = await conversationRepo.GetAsync(conversationId);

			if (conversation is null || (conversation.FirstUserId != userId && conversation.SecondUserId != userId))
				throw new NotFoundException("Conversation not found or you are not a participant.");

			var chatMessageRepo = _unitOfWork.Repository<ChatMessage>();
			var chatMessageSpecs = new ChatMessageSpecifications(conversationId, model);

			return await chatMessageRepo.GetAllWithSpecAsync(chatMessageSpecs);
		}

		public async Task<int> GetConversationMessagesCountAsync(int conversationId)
		{
			var chatMessageRepo = _unitOfWork.Repository<ChatMessage>();
			var chatMessageSpecs = new ChatMessageSpecifications(conversationId);

			return await chatMessageRepo.GetCountAsync(chatMessageSpecs);
		}

		public async Task<Conversation> GetConversationByUserIdAsync(string userId, string otherUserId)
		{
			var conversationRepo = _unitOfWork.Repository<Conversation>();
			var conversationSpecs = new ConversationSpecification(userId, otherUserId);
			var conversation = await conversationRepo.GetWithSpecAsync(conversationSpecs);

			if (conversation is null)
				throw new NotFoundException("Conversation not found or you are not a participant.");

			if (conversation.FirstUserId == userId)
				conversation.FirstUser = conversation.SecondUser;

			return conversation;
		}
	}
}
