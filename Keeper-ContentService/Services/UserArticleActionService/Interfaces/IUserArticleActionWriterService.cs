using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.UserArticleActionService.Interfaces
{
    public interface IUserArticleActionWriterService
    {
        Task<ServiceResponse<object?>> AddAsync(Guid articleId, ClaimsPrincipal user);
        Task<ServiceResponse<object?>> RemoveAsync(Guid articleId, ClaimsPrincipal user);
    }
}
