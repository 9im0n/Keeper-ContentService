using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class CategoriesRepository : BaseRepository<Categories>, ICategoriesRepository
    {
        public CategoriesRepository(AppDbContext context) : base(context) { }

        public async Task<Categories?> GetByNameAsync(string name)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
