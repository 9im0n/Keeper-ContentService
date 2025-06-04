using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.CategoryService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            ServiceResponse<ICollection<CategoryDTO>> response = await _service.GetAllAsync();
            return HandleServiceResponse(response);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            ServiceResponse<CategoryDTO?> response = await _service.GetByIdAsync(id);
            return HandleServiceResponse(response);
        }


        private IActionResult HandleServiceResponse<T>(ServiceResponse<T> response)
        {
            if (!response.IsSuccess)
                return StatusCode(statusCode: response.Status, new { message = response.Message });

            return StatusCode(statusCode: response.Status, new { data = response.Data, message = response.Message });
        }
    }
}
