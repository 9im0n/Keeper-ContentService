using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface ICommentsService
    {
        public Task<ServiceResponse<PagedResultDTO<CommentDTO>>> GetPagedAsync(Guid articleId,
            PagedRequestDTO<CommentsFilterDTO> request);
    }
}
