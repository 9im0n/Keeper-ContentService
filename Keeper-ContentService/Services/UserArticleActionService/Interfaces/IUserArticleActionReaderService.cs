using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.UserArticleActionService.Interfaces
{
    public interface IUserArticleActionReaderService<TDto, TFilter>
    {
        Task<ServiceResponse<PagedResultDTO<TDto>?>> GetPaginationAsync(
            PagedRequestDTO<TFilter> request,
            ClaimsPrincipal user);
    }
}
