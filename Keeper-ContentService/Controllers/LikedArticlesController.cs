using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.UserArticleActionService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [ApiController]
    [Route("liked-articles")]
    public class LikedArticlesController : ControllerBase
    {
        private ILikedArticleService _service;

        public LikedArticlesController(ILikedArticleService service)
        {
            _service = service;
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult>
            GetPagedLikedArticles([FromQuery] PagedRequestDTO<LikedArticlesFillterDTO> request)
        {
            ServiceResponse<PagedResultDTO<LikedArticleDTO>?> response = await _service.GetPaginationAsync(request, User);
            return HandleServiceResponse(response);
        }


        [HttpPost("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> LikeArticle(Guid id)
        {
            ServiceResponse<object?> response = await _service.AddAsync(id, User);
            return HandleServiceResponse(response);
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            ServiceResponse<object?> response = await _service.RemoveAsync(id, User);
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
