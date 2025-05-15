using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface ISavedArticlesService
    {
        public Task<ServiceResponse<PagedResultDTO<SavedArticleDTO>?>>
            GetPaginationAsync(PagedRequestDTO<SavedArticlesFillterDTO> request, ClaimsPrincipal User);
    }
}
