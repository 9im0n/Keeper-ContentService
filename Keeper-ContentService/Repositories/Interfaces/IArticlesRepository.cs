using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface IArticlesRepository : IBaseRepository<Article>
    {
        public Task<PagedResultDTO<ArticleDTO>> GetPagedArticlesAsync(PagedRequestDTO<ArticlesFillterDTO> request);
    }
}
