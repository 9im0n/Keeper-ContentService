using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface ICommentsRepository : IBaseRepository<Comments>
    {
        public Task<ICollection<Comments>> GetByUserId(Guid userId);
        public Task<ICollection<Comments>> GetByArticleId(Guid articleId);
    }
}
