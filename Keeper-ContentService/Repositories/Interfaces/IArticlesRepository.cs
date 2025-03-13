using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface IArticlesRepository : IBaseRepository<Articles>
    {
        public Task<ICollection<Articles>> GetByUserIdAsync(Guid userId);
        public Task<ICollection<Articles>> GetByCategoryIdAsync(Guid categoryId);
    }
}
