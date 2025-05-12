using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IArticlesStatusesService
    {
        public Task<ServiceResponse<ArticleStatusDTO?>> GetByIdAsync(Guid Id);
        public Task<ServiceResponse<ArticleStatusDTO?>> GetReviewStatusAsync();
        public Task<ServiceResponse<ArticleStatusDTO?>> GetDraftStatusAsync();
        public Task<ServiceResponse<ArticleStatusDTO?>> GetPublishedStatusAsync();
        public Task<ServiceResponse<ArticleStatusDTO?>> GetReadyForPublisStatusAsync();
    }
}
