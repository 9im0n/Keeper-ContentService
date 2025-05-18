using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.ProfileService.Interfaces
{
    public interface IProfileService
    {
        public Task<ServiceResponse<ProfileDTO?>> GetProfileByIdAsync(Guid id);
        public Task<ServiceResponse<ICollection<ProfileDTO>?>> GetProfilesBatchAsync(BatchedProfileRequestDTO Ids);
    }
}
