using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<ServiceResponse<PagedResultDTO<ArticleDTO>>> GetPagedAsync(PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO);
        public Task<ServiceResponse<ArticleDTO?>> GetById(Guid id);
        public Task<ServiceResponse<ArticleDTO?>> CreateDraftAsync(CreateDraftDTO createDraftDTO, ClaimsPrincipal User);
    }
}
