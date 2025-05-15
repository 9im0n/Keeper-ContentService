using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.BaseRepository.Interfaces;

namespace Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces
{
    public interface IUserArticleActionRepository<TEntity, TDto, TFilter> : IBaseRepository<TEntity>
    where TEntity : BaseModel
    {
        Task<PagedResultDTO<TDto>> GetPagedAsync(PagedRequestDTO<TFilter> request);
        Task<TEntity?> GetByUserAndArticleIdAsync(Guid userId, Guid articleId);
    }
}
