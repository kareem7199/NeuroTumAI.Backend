using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Post
{
	[Authorize]
	public class PostController : BaseApiController
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;

		public PostController(IPostService postService, IMapper mapper)
		{
			_postService = postService;
			_mapper = mapper;
		}

		[HttpPost]
		public async Task<ActionResult> AddPost(AddPostDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var post = await _postService.AddPostAsync(model, userId!);

			return Ok(new { Message = post.Id });
		}

		[HttpPost("toggleSave/{postId}")]
		public async Task<ActionResult> ToggleSavePost(int postId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var response = await _postService.ToggleSaveAsync(userId!, postId);

			return Ok(response);
		}

		[HttpPost("toggleLike/{postId}")]
		public async Task<ActionResult<ToggleLikeResponseDto>> Togglike(int postId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var result = await _postService.ToggleLikeAsync(userId, postId);

			return Ok(result);
		}

		[HttpPost("comment/{postId}")]
		public async Task<ActionResult> AddComment(int postId, AddCommentDto commentDto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var comment = await _postService.AddCommentAsync(userId, commentDto, postId);

			return Ok(new { Message = comment.Id });
		}

		[HttpGet]
		public async Task<ActionResult<CursorPaginationDto<PostToReturnDto>>> GetPosts([FromQuery] int cursor)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var posts = await _postService.GetPostsAsync(userId, cursor);

			var lastPost = posts.LastOrDefault();
			var nextCursor = lastPost?.Id ?? 0;

			var cursorPaginationDto = new CursorPaginationDto<PostToReturnDto>(nextCursor, posts);

			return Ok(cursorPaginationDto);
		}

		[HttpGet("saved")]
		public async Task<ActionResult<CursorPaginationDto<PostToReturnDto>>> GetSavedPosts([FromQuery] int cursor)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var posts = await _postService.GetSavedPostsAsync(userId, cursor);

			return Ok(posts);
		}

		[HttpGet("comments/{postId}")]
		public async Task<ActionResult<CursorPaginationDto<CommentToReturnDto>>> GetPostComments(int postId, [FromQuery] int cursor)
		{
			var comments = await _postService.GetPostCommentsAsync(postId, cursor);

			var lastComment = comments.LastOrDefault();
			var nextCursor = lastComment?.Id ?? 0;

			var cursorPaginationDto = new CursorPaginationDto<CommentToReturnDto>(nextCursor, _mapper.Map<IReadOnlyList<CommentToReturnDto>>(comments));

			return Ok(cursorPaginationDto);
		}

		[HttpDelete("{postId}")]
		public async Task<ActionResult> DeletePost(int postId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			await _postService.DeletePostAsync(userId, postId);

			return Ok(new
			{
				Message = postId
			});
		}

		[HttpDelete("comment/{commentId}")]
		public async Task<ActionResult> DeleteComment(int commentId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			await _postService.DeleteCommentAsync(userId, commentId);

			return Ok(new
			{
				Message = commentId
			});
		}
	}
}
