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
            Articles? article = await _articlesRepository.GetByIdAsync(draftId);

            if (article == null)
                return ServiceResponse<Articles?>.Fail(default, 404, "Draft with this id doesn't exist.");

            if (article.UserId != userId)
                return ServiceResponse<Articles?>.Fail(default, message: "You aren't owner of this draft.");

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
            draft = await _articlesRepository.GetByIdAsync(draft.Id);

            return ServiceResponse<Articles?>.Success(draft, 201);
        }


        public async Task<ServiceResponse<Articles?>> DeleteDraftAsync(Guid draftId, Guid userId)
        {
            Articles? draft = await _articlesRepository.GetByIdAsync(draftId);

            if (draft == null)
                return ServiceResponse<Articles?>.Fail(default, 404, "Draft doesn't exist.");

            if (draft.Statuse.Name != "draft")
                return ServiceResponse<Articles?>.Fail(default, message: "It isn't draft.");

            draft = await _articlesRepository.DeleteAsync(draftId);

            return ServiceResponse<Articles?>.Success(draft, message: "Draft has deleted.");
        }


        public async Task<ServiceResponse<Articles?>> UpdateAsync(Guid userId, Guid draftId, UpdateDraftDTO updateDraft)
        {
            Articles? draft = await _articlesRepository.GetByIdAsync(draftId);

            if (draft == null)
                return ServiceResponse<Articles?>.Fail(default, 404, "Draft doesn't exist.");

            if (draft.UserId != userId)
                return ServiceResponse<Articles?>.Fail(default, message: "You aren't owner of this draft.");

            draft.Title = updateDraft.Title;
            draft.CategoryId = draft.CategoryId;
            draft.Content = updateDraft.Content;

            draft = await _articlesRepository.UpdateAsync(draft);

            return ServiceResponse<Articles?>.Success(draft, message: "Draft has updated");
        }
    }
}
