using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.ArticleService.Interfaces
{
    public interface IArticleReaderService
    {
        Task<ServiceResponse<PagedResultDTO<ArticleDTO>>> GetPagedAsync(PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO);
        Task<ServiceResponse<ArticleDTO?>> GetByIdAsync(Guid id);
    }

}
