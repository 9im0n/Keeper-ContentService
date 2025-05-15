using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.ArticleService.Interfaces
{
    public interface IArticleEditorService
    {
        Task<ServiceResponse<ArticleDTO?>> CreateDraftAsync(CreateDraftDTO createDraftDTO, ClaimsPrincipal user);
        Task<ServiceResponse<ArticleDTO?>> UpdateArticleAsync(Guid id, UpdateArticleDTO updateArticleDTO, ClaimsPrincipal user);
        Task<ServiceResponse<object?>> DeleteAsync(Guid id, ClaimsPrincipal user);
    }

}
