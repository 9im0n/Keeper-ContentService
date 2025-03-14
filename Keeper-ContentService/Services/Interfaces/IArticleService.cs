using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<ServiceResponse<List<Articles>?>> GetDraftsByUserIdAsync(Guid userId); 
        public Task<ServiceResponse<Articles?>> GetDraftAsync(Guid userId, Guid draftId);
        public Task<ServiceResponse<Articles?>> CreateDraftAsync(DraftCreateDTO draftCreate);
        public Task<ServiceResponse<Articles?>> DeleteDraftAsync(Guid draftId, Guid userId);
        public Task<ServiceResponse<Articles?>> UpdateAsync(Guid userId, Guid draftId, UpdateDraftDTO updateDraft);
        public Task<ServiceResponse<Articles?>> MarkAsReviewAsync(Guid userId, Guid draftId);
        //public Task<ServiceResponse<List<Articles>>> GetArticlesByUserIdAsync(Guid userId);
        //public Task<ServiceResponse<Articles?>> GetArticleAsync(Guid userId, Guid draftId);
    }
}
