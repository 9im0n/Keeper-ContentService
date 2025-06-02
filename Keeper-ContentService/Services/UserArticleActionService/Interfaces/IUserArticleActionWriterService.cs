using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.UserArticleActionService.Interfaces
{
    public interface IUserArticleActionWriterService
    {
        Task<ServiceResponse<ArticleDTO?>> AddAsync(Guid articleId, ClaimsPrincipal user);
        Task<ServiceResponse<ArticleDTO?>> RemoveAsync(Guid articleId, ClaimsPrincipal user);
    }
}
