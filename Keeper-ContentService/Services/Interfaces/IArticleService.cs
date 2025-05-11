using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<ServiceResponse<PagedResultDTO<ArticleDTO>>> GetPagedAsync(PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO);
    }
}
