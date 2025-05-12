using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface ICategoriesService
    {
        public Task<ServiceResponse<ICollection<CategoryDTO>>> GetAllAsync();
        public Task<ServiceResponse<CategoryDTO?>> GetByIdAsync(Guid Id);
    }
}
