using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Keeper_ContentService.Services.UserArticleActionService.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.UserArticleActionService.Implementations
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly ISavedArticlesRepository _repository;
        private readonly IArticleService _articleService;
        private readonly IDTOMapperService _mapper;

        public SavedArticleService(
            ISavedArticlesRepository repository,
            IArticleService articleService,
            IDTOMapperService mapper)
        {
            _repository = repository;
            _articleService = articleService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PagedResultDTO<SavedArticleDTO>?>>
            GetPaginationAsync(PagedRequestDTO<SavedArticlesFillterDTO> request, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<PagedResultDTO<SavedArticleDTO>?>.Fail(default, 401, "User unauthorized");

            if (request.Filter?.UserId != userId && User.FindFirst(ClaimTypes.Role)?.Value == "user")
                return ServiceResponse<PagedResultDTO<SavedArticleDTO>?>.Fail(default, 403, "You don't have required permissions.");

            PagedResultDTO<SavedArticleDTO> response = await _repository.GetPagedAsync(request);

            return ServiceResponse<PagedResultDTO<SavedArticleDTO>?>.Success(response);
        }


        public async Task<ServiceResponse<object?>> AddAsync(Guid articleId, ClaimsPrincipal User)
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


        public async Task<ServiceResponse<object?>> RemoveAsync(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<object?>.Fail(default, 401, "User unauthorized");

            SavedArticle? savedArticle = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (savedArticle == null)
                return ServiceResponse<object?>.Fail(default, 404, "You didn't save this article.");

            await _repository.DeleteAsync(savedArticle.Id);

            return ServiceResponse<object?>.Success(null);
        }
    }
}
