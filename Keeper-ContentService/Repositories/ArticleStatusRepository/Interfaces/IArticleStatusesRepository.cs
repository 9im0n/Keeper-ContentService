using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.BaseRepository.Interfaces;

namespace Keeper_ContentService.Repositories.ArticleStatusRepository.Interfaces
{
    public interface IArticleStatusesRepository : IBaseRepository<ArticleStatus>
    {
        public Task<ArticleStatus?> GetByNameAsync(string name);
    }
}
