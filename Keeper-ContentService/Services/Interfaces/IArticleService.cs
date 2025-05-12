using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<ServiceResponse<PagedResultDTO<ArticleDTO>>> GetPagedAsync(PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO);
        public Task<ServiceResponse<ArticleDTO?>> GetByIdAsync(Guid id);

        public Task<ServiceResponse<ArticleDTO?>> CreateDraftAsync(CreateDraftDTO createDraftDTO, 
            ClaimsPrincipal User);

        public Task<ServiceResponse<ArticleDTO?>> UpdateArticleAsync(Guid id, 
            UpdateArticleDTO updateArticleDTO,
            ClaimsPrincipal User);

        public Task<ServiceResponse<object?>> DeleteAsync(Guid id, ClaimsPrincipal User);
    }
}
