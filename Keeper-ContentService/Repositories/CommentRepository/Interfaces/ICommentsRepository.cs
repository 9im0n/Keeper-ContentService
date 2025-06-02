using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.BaseRepository.Interfaces;

namespace Keeper_ContentService.Repositories.CommentRepository.Interfaces
{
    public interface ICommentsRepository : IBaseRepository<Comment>
    {
        public Task<PagedResultDTO<Comment>> GetPagedCommentsAsync(Guid articleId,
            PagedRequestDTO<CommentsFilterDTO> request);
    }
}
