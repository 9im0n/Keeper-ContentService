using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface ICategoriesRepository : IBaseRepository<Categories>
    {
        public Task<Categories?> GetByNameAsync(string name);
    }
}
