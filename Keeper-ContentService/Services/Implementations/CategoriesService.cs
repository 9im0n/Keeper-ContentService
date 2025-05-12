using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Keeper_ContentService.Services.Implementations
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _repository;
        private readonly IDTOMapperService _mapper;

        public CategoriesService(ICategoriesRepository repository,
            IDTOMapperService mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ICollection<CategoryDTO>>> GetAllAsync()
        {
            ICollection<Category> categories = await _repository.GetAllAsync();
            ICollection<CategoryDTO> categoryDTOs = _mapper.Map(categories);

            return ServiceResponse<ICollection<CategoryDTO>>.Success(categoryDTOs);
        }

        public async Task<ServiceResponse<CategoryDTO?>> GetByIdAsync(Guid Id)
        {
            Category? category = await _repository.GetByIdAsync(Id);

            if (category == null) 
                return ServiceResponse<CategoryDTO?>.Success(default, 404, "Category doesn't exist.");

            CategoryDTO categoryDTO = _mapper.Map(category);
            return ServiceResponse<CategoryDTO?>.Success(categoryDTO);
        }
    }
}
