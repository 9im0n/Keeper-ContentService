using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface ICommentsRepository : IBaseRepository<Comment>
    {
        public Task<PagedResultDTO<CommentDTO>> GetPagedCommentsAsync(Guid articleId,
            PagedRequestDTO<CommentsFilterDTO> request);
    }
}
