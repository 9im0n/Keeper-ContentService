using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            ServiceResponse<PagedResultDTO<ArticleDTO>> response = await _articleService.GetPagedAsync(pagedRequestDTO);
            return HandleServiceResponse(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid articleId)
        {
            ServiceResponse<ArticleDTO?> resppnse = await _articleService.GetById(articleId);
            return HandleServiceResponse(resppnse);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDraft([FromBody] CreateDraftDTO createDraftDTO)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateArticle(Guid articleId, [FromBody] UpdateArticleDTO updateArticleDTO)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }

        private IActionResult HandleServiceResponse<T>(ServiceResponse<T> response)
        {
            if (!response.IsSuccess)
                return StatusCode(response.Status, new { message = response.Message });

            return Ok(new { data = response.Data, message = response.Message });
        }
    }
}
