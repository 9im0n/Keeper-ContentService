using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.ArticleStatusRepository.Interfaces;
using Keeper_ContentService.Repositories.BaseRepository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.ArticleStatusRepository.Implementations
{
    public class ArticleStatusRepository : BaseRepository<ArticleStatus>, IArticleStatusesRepository
    {
        public ArticleStatusRepository(AppDbContext context) : base(context) { }

        public async Task<ArticleStatus?> GetByNameAsync(string name)
        {
            return await _appDbContext.ArticleStatuses.FirstOrDefaultAsync(a => a.Name == name);
        }
    }
}
