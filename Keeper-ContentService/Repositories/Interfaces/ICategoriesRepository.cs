using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface ICategoriesRepository : IBaseRepository<Category>
    {
        public Task<Category?> GetByNameAsync(string name);
    }
}
