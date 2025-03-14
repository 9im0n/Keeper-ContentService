using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class ArticlesRepository : BaseRepository<Articles>, IArticlesRepository
    {
        public ArticlesRepository(AppDbContext context) : base(context) { }

        public async Task<ICollection<Articles>> GetByUserIdAsync(Guid userId)
        {
            return await _appDbContext.Articles.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<Articles>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _appDbContext.Articles.Where(a => a.Category.Id == categoryId).ToListAsync();
        }

        public async Task<Articles?> GetArticleByUserIdAsync(Guid userId, Guid draftId)
        {
            return await _appDbContext.Articles.FirstOrDefaultAsync(a => a.Id == draftId && a.UserId == userId);
        }
    }
}
