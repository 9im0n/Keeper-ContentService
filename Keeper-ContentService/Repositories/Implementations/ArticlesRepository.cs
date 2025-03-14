using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class ArticlesRepository : BaseRepository<Articles>, IArticlesRepository
    {
        public ArticlesRepository(AppDbContext context) : base(context) { }

        public override async Task<ICollection<Articles>> GetAllAsync()
        {
            return await _appDbContext.Articles.Include(a => a.Statuse).Include(a => a.Category)
                .ToListAsync();
        }

        public override async Task<Articles?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Articles.Include(a => a.Statuse).Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ICollection<Articles>> GetByUserIdAsync(Guid userId)
        {
            return await _appDbContext.Articles.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<Articles>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _appDbContext.Articles.Where(a => a.Category.Id == categoryId).ToListAsync();
        }
    }
}
