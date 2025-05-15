using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface ISavedArticlesRepository : IBaseRepository<SavedArticle>
    {
        public Task<PagedResultDTO<SavedArticleDTO>>
            GetPagedLikedArticlesAsync(PagedRequestDTO<SavedArticlesFillterDTO> request);

        public Task<SavedArticle?> GetByUserAndArticleIdAsync(Guid userId, Guid articleId);
    }
}
