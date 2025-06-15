using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities.Post_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.PostSpecs;
using NeuroTumAI.Core.Specifications.PostSpecs.CommentSpecs;
using NeuroTumAI.Core.Specifications.PostSpecs.LikeSpecs;
using NeuroTumAI.Core.Specifications.PostSpecs.SavedPostSpecs;
using NeuroTumAI.Service.Hubs;

namespace NeuroTumAI.Service.Services.PostService
{
	public class PostService : IPostService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHubContext<PostHub> _hubContext;
		private readonly ILocalizationService _localizationService;
		private readonly IMapper _mapper;

		public PostService(IUnitOfWork unitOfWork, IHubContext<PostHub> hubContext, ILocalizationService localizationService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_hubContext = hubContext;
			_localizationService = localizationService;
			_mapper = mapper;
		}

		public async Task<Comment> AddCommentAsync(string userId, AddCommentDto model, int postId)
		{
			var postRepo = _unitOfWork.Repository<Post>();
			var postSpec = new PostSpecifications(postId);
			var post = await postRepo.GetWithSpecAsync(postSpec);

			if (post is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

			var likesCount = post.Likes.Count();
			var commentsCount = post.Comments.Count() + 1;

			var commentRepo = _unitOfWork.Repository<Comment>();
			var newComment = new Comment()
			{
				PostId = postId,
				ApplicationUserId = userId,
				Text = model.Text
			};

			commentRepo.Add(newComment);

			await _unitOfWork.CompleteAsync();

			await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", new
			{
				PostId = postId,
				likesCount,
				commentsCount
			});

			return newComment;
		}

		public async Task<Post> AddPostAsync(AddPostDto model, string applicationUserId)
		{
			var newPost = new Post()
			{
				Title = model.Title,
				Content = model.Content,
				ApplicationUserId = applicationUserId
			};

			var postRepo = _unitOfWork.Repository<Post>();

			postRepo.Add(newPost);
			await _unitOfWork.CompleteAsync();

			return newPost;
		}

		public async Task DeleteCommentAsync(string userId, int commentId)
		{
			var commentRepo = _unitOfWork.Repository<Comment>();
			var comment = await commentRepo.GetAsync(commentId);

			if (comment is null || comment.ApplicationUserId != userId)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("CommentNotFound"));

			commentRepo.Delete(comment);

			await _unitOfWork.CompleteAsync();

			var postRepo = _unitOfWork.Repository<Post>();
			var postSpec = new PostSpecifications(comment.PostId);
			var post = await postRepo.GetWithSpecAsync(postSpec);

			var likesCount = post!.Likes.Count();
			var commentsCount = post!.Comments.Count();

			await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", new
			{
				comment.PostId,
				likesCount,
				commentsCount
			});
		}

		public async Task DeletePostAsync(string userId, int postId)
		{
			var postRepo = _unitOfWork.Repository<Post>();
			var postSpecs = new PostSpecifications(postId);
			var post = await postRepo.GetWithSpecAsync(postSpecs);

			if (post is null || post.ApplicationUserId != userId)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

			var commentRepo = _unitOfWork.Repository<Comment>();
			var likeRepo = _unitOfWork.Repository<Like>();
			var savedPostRepo = _unitOfWork.Repository<SavedPost>();

			commentRepo.RemoveRange(post.Comments);
			likeRepo.RemoveRange(post.Likes);
			savedPostRepo.RemoveRange(post.Saves);

			postRepo.Delete(post);

			await _unitOfWork.CompleteAsync();
		}

		public async Task<IReadOnlyList<Comment>> GetPostCommentsAsync(int postId, int cursor)
		{
			var post = await _unitOfWork.Repository<Post>().GetAsync(postId);
			if (post is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

			var commentSpecs = new CommentCursorPaginationSpecifications(cursor, postId);
			var comments = await _unitOfWork.Repository<Comment>().GetAllWithSpecAsync(commentSpecs);

			return comments;
		}

		public async Task<IReadOnlyList<PostToReturnDto>> GetPostsAsync(string userId, int cursor)
		{
			var postSpecs = new PostCursorPaginationSpecifications(cursor);
			var posts = await _unitOfWork.Repository<Post>().GetAllWithSpecAsync(postSpecs);

			return await FormatPosts(userId, posts);
		}

		public async Task<IReadOnlyList<PostToReturnDto>> FormatPosts(string userId, IReadOnlyList<Post> posts)
		{
			var postIds = posts.Select(P => P.Id).ToList();

			var likeSpecs = new LikeByUserAndPostSpecification(userId, postIds);
			var likes = await _unitOfWork.Repository<Like>().GetAllWithSpecAsync(likeSpecs);

			var savedPostSpecs = new SavedPostSpecifications(userId, postIds);
			var savedPosts = await _unitOfWork.Repository<SavedPost>().GetAllWithSpecAsync(savedPostSpecs);

			var postsDto = _mapper.Map<IReadOnlyList<PostToReturnDto>>(posts);

			foreach (var post in postsDto)
			{
				post.IsLiked = likes.Any(L => L.PostId == post.Id);
				post.IsSaved = savedPosts.Any(L => L.PostId == post.Id);
			}

			return postsDto;
		}

		public async Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId)
		{
			var postRepo = _unitOfWork.Repository<Post>();
			var postSpec = new PostSpecifications(postId);
			var post = await postRepo.GetWithSpecAsync(postSpec);

			if (post is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

			var likesCount = post.Likes.Count();
			var commentsCount = post.Comments.Count();

			var likeRepo = _unitOfWork.Repository<Like>();

			var likeSpec = new LikeByUserAndPostSpecification(userId, postId);
			var existingLike = await likeRepo.GetWithSpecAsync(likeSpec);
			if (existingLike is not null)
			{
				likeRepo.Delete(existingLike);
				likesCount--;
			}
			else
			{
				var newLike = new Like()
				{
					ApplicationUserId = userId,
					PostId = postId
				};
				likeRepo.Add(newLike);
				likesCount++;
			}

			await _unitOfWork.CompleteAsync();

			await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", new
			{
				PostId = postId,
				likesCount,
				commentsCount
			});

			return new ToggleLikeResponseDto()
			{
				IsLiked = existingLike is null,
				PostId = postId
			};
		}

		public async Task<ToggleSaveResponseDto> ToggleSaveAsync(string userId, int postId)
		{
			var postRepo = _unitOfWork.Repository<Post>();
			var postSpec = new PostSpecifications(postId);
			var post = await postRepo.GetWithSpecAsync(postSpec);

			if (post is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

			var savedPostRepo = _unitOfWork.Repository<SavedPost>();

			var savedPostSpec = new SavedPostSpecifications(userId, postId);
			var isSaved = await savedPostRepo.GetWithSpecAsync(savedPostSpec);

			if (isSaved is not null)
			{
				savedPostRepo.Delete(isSaved);
			}
			else
			{
				var newSavedPost = new SavedPost()
				{
					ApplicationUserId = userId,
					PostId = postId
				};
				savedPostRepo.Add(newSavedPost);
			}

			await _unitOfWork.CompleteAsync();

			return new ToggleSaveResponseDto()
			{
				IsSaved = isSaved is null,
				PostId = postId
			};
		}

		public async Task<CursorPaginationDto<PostToReturnDto>> GetSavedPostsAsync(string userId, int cursor)
		{
			var savedPostSpecs = new SavedPostCursorPaginationSpecifications(userId, cursor);
			var savedPosts = await _unitOfWork.Repository<SavedPost>().GetAllWithSpecAsync(savedPostSpecs);
			var posts = new List<Post>();

			foreach (var savedPost in savedPosts) {
				posts.Add(savedPost.Post);
			}

			var formattedPosts = await FormatPosts(userId, posts);


			var lastPost = savedPosts.LastOrDefault();
			var nextCursor = lastPost?.Id ?? 0;

			return new CursorPaginationDto<PostToReturnDto>(nextCursor, formattedPosts);
		}
	}
}
