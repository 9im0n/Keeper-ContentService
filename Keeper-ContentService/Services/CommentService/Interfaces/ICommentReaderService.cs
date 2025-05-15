using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.CommentService.Interfaces
{
    public interface ICommentReaderService
    {
        Task<ServiceResponse<PagedResultDTO<CommentDTO>?>> GetPagedAsync(
            Guid articleId,
            PagedRequestDTO<CommentsFilterDTO> request);
    }
}
