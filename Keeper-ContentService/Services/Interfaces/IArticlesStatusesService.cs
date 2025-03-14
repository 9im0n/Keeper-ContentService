using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IArticlesStatusesService
    {
        public Task<ServiceResponse<ArticleStatuses?>> GetByIdAsync(Guid Id);
        public Task<ServiceResponse<ArticleStatuses?>> GetReviewStatusAsync();
        public Task<ServiceResponse<ArticleStatuses?>> GetDraftStatusAsync();
        public Task<ServiceResponse<ArticleStatuses?>> GetPublishedStatusAsync();
        public Task<ServiceResponse<ArticleStatuses?>> GetReadyForPublisStatusAsync();
    }
}
