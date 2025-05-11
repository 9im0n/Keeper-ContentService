using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface IArticleStatusesRepository : IBaseRepository<ArticleStatus>
    {
        public Task<ArticleStatus?> GetByNameAsync(string name);
    }
}
