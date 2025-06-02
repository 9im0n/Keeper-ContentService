using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.UserArticleActionService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [Route("saved-articles")]
    public class SavedArticlesController : Controller
    {
        private readonly ISavedArticleService _service;

        public SavedArticlesController(
            ISavedArticleService service
            )
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult>
            GetPagedSavedArticles([FromQuery] PagedRequestDTO<SavedArticlesFillterDTO> request)
        {
            ServiceResponse<PagedResultDTO<ArticleDTO>?> response = await _service.GetPaginationAsync(request, User);
            return HandleServiceResponse(response);
        }


        [HttpPost("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> SaveArticle(Guid id)
        {
            ServiceResponse<ArticleDTO?> response = await _service.AddAsync(id, User);
            return HandleServiceResponse(response);
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteSavedArticle(Guid id)
        {
            ServiceResponse<ArticleDTO?> response = await _service.RemoveAsync(id, User);
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
