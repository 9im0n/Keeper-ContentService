using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.ArticleService.Interfaces
{
    public interface IArticleReaderService
    {
        Task<ServiceResponse<PagedResultDTO<ArticleDTO>?>> GetPagedAsync(PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO,
            ClaimsPrincipal? User);
        Task<ServiceResponse<ArticleDTO?>> GetByIdAsync(Guid id, ClaimsPrincipal? User = null);
    }

}
