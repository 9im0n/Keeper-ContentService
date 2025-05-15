using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.UserArticleActionService.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Keeper_ContentService.Services.UserArticleActionService.Implementations
{
    public class LikedArticleService : ILikedArticleService
    {
        private readonly IArticleService _articleService;
        private readonly ILikedArticlesRepository _repository;
        private readonly ILogger<LikedArticleService> _logger;

        public LikedArticleService(
            IArticleService articleService,
            ILikedArticlesRepository repository,
            ILogger<LikedArticleService> logger)
        {
            _articleService = articleService;
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<PagedResultDTO<LikedArticleDTO>?>>
            GetPaginationAsync(PagedRequestDTO<LikedArticlesFillterDTO> request, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<PagedResultDTO<LikedArticleDTO>?>.Fail(default, 401, "User unauthorized");

            _logger.LogInformation(userId.ToString());
            _logger.LogInformation(request.Filter?.UserId.ToString());

            if (request.Filter?.UserId != userId && User.FindFirst(ClaimTypes.Role)?.Value == "user")
                return ServiceResponse<PagedResultDTO<LikedArticleDTO>?>.Fail(default, 403, "You don't have required permissions.");

            PagedResultDTO<LikedArticleDTO> response = await _repository.GetPagedAsync(request);

            return ServiceResponse<PagedResultDTO<LikedArticleDTO>?>.Success(response);
        }


        public async Task<ServiceResponse<object?>> AddAsync(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<object?>.Fail(default, 401, "User unauthorized");

            LikedArticle? liked = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (liked != null)
                return ServiceResponse<object?>.Fail(default, 409, "You liked it before.");

            LikedArticle newLikedarticle = new LikedArticle()
            {
                ArticleId = articleId,
                UserId = userId,
            };

            newLikedarticle = await _repository.CreateAsync(newLikedarticle);

            return ServiceResponse<object?>.Success(default);
        }


        public async Task<ServiceResponse<object?>> RemoveAsync(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<object?>.Fail(default, 401, "User unauthorized");

            LikedArticle? liked = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (liked == null)
                return ServiceResponse<object?>.Fail(default, 404, "Liked article doesn't exist.");

            await _repository.DeleteAsync(liked.Id);

            return ServiceResponse<object?>.Success(default);
        }
    }
}
