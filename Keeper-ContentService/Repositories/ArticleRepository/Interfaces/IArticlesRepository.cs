using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.BaseRepository.Interfaces;

namespace Keeper_ContentService.Repositories.ArticleRepository.Interfaces
{
    public interface IArticlesRepository : IBaseRepository<Article>
    {
        public Task<PagedResponseDTO<Article>> GetPagedArticlesAsync(PagedRequestDTO<ArticlesFillterDTO> request);
    }
}
