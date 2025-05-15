using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface ILikedArticlesRepository : IBaseRepository<LikedArticle>
    {
        public Task<PagedResultDTO<LikedArticleDTO>>
            GetPagedLikedArticlesAsync(PagedRequestDTO<LikedArticlesFillterDTO> request);

        public Task<LikedArticle?> GetByUserAndArticleId(Guid userId, Guid articleId);
    }
}
