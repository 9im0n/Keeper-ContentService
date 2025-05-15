using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.ArticleStatusService.Interfaces
{
    public interface IArticleStatusReaderService
    {
        Task<ServiceResponse<ICollection<ArticleStatusDTO>>> GetAllAsync();
        Task<ServiceResponse<ArticleStatusDTO?>> GetByIdAsync(Guid id);
    }
}
