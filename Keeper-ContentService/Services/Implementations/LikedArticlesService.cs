using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Implementations
{
    public class LikedArticlesService : ILikedArticlesService
    {
        private readonly IArticleService _articleService;
        private readonly ILikedArticlesRepository _repository;
        private readonly ILogger<LikedArticlesService> _logger;

        public LikedArticlesService(
            IArticleService articleService,
            ILikedArticlesRepository repository,
            ILogger<LikedArticlesService> logger)
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

            if ((request.Filter?.UserId != userId) && (User.FindFirst(ClaimTypes.Role)?.Value == "user"))
                return ServiceResponse<PagedResultDTO<LikedArticleDTO>?>.Fail(default, 403, "You don't have required permissions.");

            PagedResultDTO<LikedArticleDTO> response = await _repository.GetPagedLikedArticlesAsync(request);

            return ServiceResponse<PagedResultDTO<LikedArticleDTO>?>.Success(response);
        }
    }
}
