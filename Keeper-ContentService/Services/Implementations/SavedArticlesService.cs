using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Implementations
{
    public class SavedArticlesService : ISavedArticlesService
    {
        private readonly ISavedArticlesRepository _repository;

        public SavedArticlesService(ISavedArticlesRepository repository)
        {
            _repository = repository;
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
    }
}
