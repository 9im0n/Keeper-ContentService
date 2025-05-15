using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Implementations
{
    public class SavedArticlesService : ISavedArticlesService
    {
        private readonly ISavedArticlesRepository _repository;
        private readonly IArticleService _articleService;

        public SavedArticlesService(
            ISavedArticlesRepository repository,
            IArticleService articleService)
        {
            _repository = repository;
            _articleService = articleService;
        }

        public async Task<ServiceResponse<PagedResultDTO<SavedArticleDTO>?>>
            GetPaginationAsync(PagedRequestDTO<SavedArticlesFillterDTO> request, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<PagedResultDTO<SavedArticleDTO>?>.Fail(default, 401, "User unauthorized");

            if ((request.Filter?.UserId != userId) && (User.FindFirst(ClaimTypes.Role)?.Value == "user"))
                return ServiceResponse<PagedResultDTO<SavedArticleDTO>?>.Fail(default, 403, "You don't have required permissions.");

            PagedResultDTO<SavedArticleDTO> response = await _repository.GetPagedLikedArticlesAsync(request);

            return ServiceResponse<PagedResultDTO<SavedArticleDTO>?>.Success(response);
        }


        public async Task<ServiceResponse<object?>> SaveArticle(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<object?>.Fail(default, 401, "User unauthorized");

            ServiceResponse<ArticleDTO?> articleServiceResponse = await _articleService.GetByIdAsync(articleId);

            if (!articleServiceResponse.IsSuccess)
                return ServiceResponse<object?>.Fail(default, articleServiceResponse.Status, articleServiceResponse.Message);

            SavedArticle? savedArticle = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (savedArticle != null)
                return ServiceResponse<object?>.Fail(default, 409, "You already saved it.");

            savedArticle = new SavedArticle()
            {
                ArticleId = articleId,
                UserId = userId,
            };

            await _repository.CreateAsync(savedArticle);

            return ServiceResponse<object?>.Success(null);
        }
    }
}
