using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface ILikedArticlesService
    {
        public Task<ServiceResponse<PagedResultDTO<LikedArticleDTO>?>>
            GetPaginationAsync(PagedRequestDTO<LikedArticlesFillterDTO> request, ClaimsPrincipal User);
    }
}
