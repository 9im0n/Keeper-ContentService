using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IArticlesStatusesService
    {
        public Task<ServiceResponse<ArticleStatus?>> GetByIdAsync(Guid Id);
        public Task<ServiceResponse<ArticleStatus?>> GetReviewStatusAsync();
        public Task<ServiceResponse<ArticleStatus?>> GetDraftStatusAsync();
        public Task<ServiceResponse<ArticleStatus?>> GetPublishedStatusAsync();
        public Task<ServiceResponse<ArticleStatus?>> GetReadyForPublisStatusAsync();
    }
}
