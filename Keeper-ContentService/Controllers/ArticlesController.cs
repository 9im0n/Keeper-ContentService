using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Keeper_ContentService.Controllers
{
    [ApiController]
    [Route("articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedArticles([FromQuery] PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO)
        {
            ServiceResponse<PagedResultDTO<ArticleDTO>?> response = await _articleService.GetPagedAsync(pagedRequestDTO, User);
            return HandleServiceResponse(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GeArticletById(Guid id)
        {
            ServiceResponse<ArticleDTO?> resppnse = await _articleService.GetByIdAsync(id, User);
            return HandleServiceResponse(resppnse);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDraft([FromBody] CreateDraftDTO createDraftDTO)
        {
            ServiceResponse<ArticleDTO?> response = await _articleService.CreateDraftAsync(createDraftDTO, User);
            return HandleServiceResponse(response);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateArticle(Guid id, [FromBody] UpdateArticleDTO updateArticleDTO)
        {
            ServiceResponse<ArticleDTO?> response = await _articleService
                .UpdateArticleAsync(id, updateArticleDTO, User);
            return HandleServiceResponse(response);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            ServiceResponse<object?> response = await _articleService.DeleteAsync(id, User);
            return HandleServiceResponse(response);
        }

        private IActionResult HandleServiceResponse<T>(ServiceResponse<T> response)
        {
            if (!response.IsSuccess)
                return StatusCode(response.Status, new { message = response.Message });

            return StatusCode(response.Status ,new { data = response.Data, message = response.Message });
        }
    }
}
