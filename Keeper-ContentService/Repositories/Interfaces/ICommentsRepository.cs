using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface ICommentsRepository : IBaseRepository<Comment>
    {
        public Task<ICollection<Comment>> GetByUserId(Guid userId);
        public Task<ICollection<Comment>> GetByArticleId(Guid articleId);
    }
}
