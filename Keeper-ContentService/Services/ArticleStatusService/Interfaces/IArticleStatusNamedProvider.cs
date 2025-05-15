using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.ArticleStatusService.Interfaces
{
    public interface IArticleStatusNamedProvider
    {
        Task<ServiceResponse<ArticleStatusDTO?>> GetDraftStatusAsync();
        Task<ServiceResponse<ArticleStatusDTO?>> GetReviewStatusAsync();
        Task<ServiceResponse<ArticleStatusDTO?>> GetPublishedStatusAsync();
        Task<ServiceResponse<ArticleStatusDTO?>> GetReadyForPublisStatusAsync();
    }

}
