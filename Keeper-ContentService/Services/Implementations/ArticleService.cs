using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;

namespace Keeper_ContentService.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticlesRepository _articlesRepository;

        public ArticleService(IArticlesRepository articlesRepository)
        {
            _articlesRepository = articlesRepository;
        }

        public async Task<ServiceResponse<List<Articles>?>> GetDraftsByUserIdAsync(Guid userId)
        {
            List<Articles>? articles = await _articlesRepository.GetByUserIdAsync(userId) as List<Articles>;

            return ServiceResponse<List<Articles>?>.Success(articles);
        }


        public async Task<ServiceResponse<Articles?>> GetDraftAsync(Guid userId, Guid draftId)
        {
            Articles? article = await _articlesRepository.GetArticleByUserIdAsync(userId, draftId);

            if (article == null)
                return ServiceResponse<Articles?>.Fail(default, 404, "Draft with this id doesn't exist.");

            return ServiceResponse<Articles?>.Success(article);
        }


        public async Task<ServiceResponse<Articles?>> CreateDraftAsync(DraftCreateDTO draftCreate)
        {
            Articles? draft = new Articles()
            {
                Title = draftCreate.Title,
                CategoryId = draftCreate.CategoryId,
                UserId = draftCreate.UserId,
                Content = draftCreate.Content,
                StatuseId = draftCreate.StatuseId,
                CreatedAt = draftCreate.CreatedAt,
            };

            draft = await _articlesRepository.CreateAsync(draft);

            return ServiceResponse<Articles?>.Success(draft);
        }
    }
}
