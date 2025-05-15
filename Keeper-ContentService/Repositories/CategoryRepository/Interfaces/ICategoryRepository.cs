using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.BaseRepository.Interfaces;

namespace Keeper_ContentService.Repositories.CategoryRepository.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        public Task<Category?> GetByNameAsync(string name);
    }
}
