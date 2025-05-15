using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.BaseRepository.Implementations;
using Keeper_ContentService.Repositories.CategoryRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.CategoryRepository.Implementations
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
