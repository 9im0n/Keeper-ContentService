using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.BaseRepository.Interfaces;

namespace Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces
{
    public interface IUserArticleActionRepository<TEntity, TFilter> : IBaseRepository<TEntity>
    where TEntity : BaseModel
    {
        public Task<PagedResultDTO<TEntity>> GetPagedAsync(PagedRequestDTO<TFilter> request);
        public Task<TEntity?> GetByUserAndArticleIdAsync(Guid userId, Guid articleId);
        public Task<ICollection<TEntity>> GetBatchedByUserAndArticleId(ICollection<Guid> articleIds, Guid userId);
    }
}
