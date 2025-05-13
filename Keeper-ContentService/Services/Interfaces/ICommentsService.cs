using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface ICommentsService
    {
        public Task<ServiceResponse<PagedResultDTO<CommentDTO>?>> GetPagedAsync(Guid articleId,
            PagedRequestDTO<CommentsFilterDTO> request);

        public Task<ServiceResponse<CommentDTO?>> CreateAsync(Guid articleId,
            CreateCommentDTO createCommentDTO, ClaimsPrincipal User);
    }
}
