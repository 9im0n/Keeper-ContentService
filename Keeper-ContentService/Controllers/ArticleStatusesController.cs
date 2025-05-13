using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [Route("article-statuses")]
    public class ArticleStatusesController : Controller
    {
        private readonly IArticlesStatusesService _service;

        public ArticleStatusesController(IArticlesStatusesService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllArticleStatuses()
        {
            ServiceResponse<ICollection<ArticleStatusDTO>> response = await _service.GetAllAsync();
            return HandleServiceResponse(response);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetArticleStatusById(Guid id)
        {
            ServiceResponse<ArticleStatusDTO?> response = await _service.GetByIdAsync(id);
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
