using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [Route("saved-articles")]
    public class SavedArticlesController : Controller
    {
        private readonly ISavedArticlesService _service;

        public SavedArticlesController(
            ISavedArticlesService service
            )
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult>
            GetPagination([FromQuery] PagedRequestDTO<SavedArticlesFillterDTO> request)
        {
            ServiceResponse<PagedResultDTO<SavedArticleDTO>?> response = await _service.GetPaginationAsync(request, User);
            return HandleServiceResponse(response);
        }


        [HttpPost("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> SaveArticle(Guid id)
        {
            ServiceResponse<object?> response = await _service.SaveArticle(id, User);
            return HandleServiceResponse(response);
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteSavedArticle(Guid id)
        {
            ServiceResponse<object?> response = await _service.DeleteFromSaved(id, User);
            return HandleServiceResponse(response);
        }

        private IActionResult HandleServiceResponse<T>(ServiceResponse<T> response)
        {
            if (!response.IsSuccess)
                return StatusCode(statusCode: response.Status, new { message = response.Message });

            return StatusCode(statusCode: response.Status, new { data = response.Data, response.Message });
        }
    }
}
