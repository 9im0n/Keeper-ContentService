using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.CategoryService.Interfaces
{
    public interface ICategoryReaderService
    {
        public Task<ServiceResponse<ICollection<CategoryDTO>>> GetAllAsync();
        public Task<ServiceResponse<CategoryDTO?>> GetByIdAsync(Guid Id);
    }
}
