using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [ApiController]
    [Route("liked-articles")]
    public class LikedArticlesController : ControllerBase
    {
        private ILikedArticlesService _service;

        public LikedArticlesController(ILikedArticlesService service)
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


        private IActionResult HandleServiceResponse<T>(ServiceResponse<T> response)
        {
            if (!response.IsSuccess)
                return StatusCode(statusCode: response.Status, new { message = response.Message });

            return StatusCode(statusCode: response.Status, new { data = response.Data, message = response.Message });
        }
    }
}
