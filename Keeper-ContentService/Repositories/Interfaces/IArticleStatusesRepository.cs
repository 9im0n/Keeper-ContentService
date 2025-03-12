using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface IArticleStatusesRepository : IBaseRepository<ArticleStatuses>
    {
        public Task<ArticleStatuses?> GetByNameAsync(string name);
    }
}
