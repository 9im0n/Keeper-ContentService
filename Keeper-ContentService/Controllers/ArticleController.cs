using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [Route("articles")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }


        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAllArticles(Guid id)
        {
            try
            {
                ServiceResponse<Articles?> response = await _articleService.GetArticleById(id);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}.");
            }
        }
    }
}
