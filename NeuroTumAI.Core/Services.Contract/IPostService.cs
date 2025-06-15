using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IPostService
	{
		Task<Post> AddPostAsync(AddPostDto model, string applicationUserId);
		Task<Comment> AddCommentAsync(string userId, AddCommentDto model, int postId);
		Task<IReadOnlyList<PostToReturnDto>> GetPostsAsync(string userId, int cursor);
		Task<CursorPaginationDto<PostToReturnDto>> GetSavedPostsAsync(string userId, int cursor);
		Task<IReadOnlyList<Comment>> GetPostCommentsAsync(int postId, int cursor);
		Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId);
		Task<ToggleSaveResponseDto> ToggleSaveAsync(string userId, int postId);
		Task DeletePostAsync(string userId, int postId);
		Task DeleteCommentAsync(string userId, int commentId);
	}
}
