using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class ArticleStatusesRepository : BaseRepository<ArticleStatuses>, IArticleStatusesRepository
    {
        public ArticleStatusesRepository(AppDbContext context) : base(context) { }

        public async Task<ArticleStatuses?> GetByNameAsync(string name)
        {
            return await _appDbContext.ArticleStatuses.FirstOrDefaultAsync(a => a.Name == name);
        }
    }
}
