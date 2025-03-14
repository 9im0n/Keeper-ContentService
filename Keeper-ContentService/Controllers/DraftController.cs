using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.Implementations;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Keeper_ContentService.Controllers
{
    [Route("drafts")]
    public class DraftController : Controller
    {
        private readonly IArticleService _articleService;

        public DraftController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUserDrafts()
        {
            Guid userId = new Guid(User.FindFirst("UserId")?.Value);

            try
            {
                ServiceResponse<List<Articles>> response = await _articleService.GetDraftsByUserIdAsync(userId);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}");
            }
        }


        [Authorize]
        [HttpGet("{draftId:guid}")]
        public async Task<IActionResult> GetById(Guid draftId)
        {
            Guid userId = new Guid(User.FindFirst("UserId")?.Value);

            try
            {
                ServiceResponse<Articles> response = await _articleService.GetDraftAsync(userId, draftId);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DraftCreateDTO draft)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Guid userId = new Guid(User.FindFirst("UserId")?.Value);

            if (draft.UserId != userId)
                return BadRequest(new { message = "Ids don't match." });

            try
            {
                ServiceResponse<Articles?> response = await _articleService.CreateDraftAsync(draft);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}\n{ex.InnerException}");
            }
        }


        [Authorize]
        [HttpDelete("{draftId:guid}")]
        public async Task<IActionResult> Delete(Guid draftId)
        {
            Guid userId = new Guid(User.FindFirst("UserId").Value);

            try
            {
                ServiceResponse<Articles?> response = await _articleService.DeleteDraftAsync(draftId, userId);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}\n{ex.InnerException}");
            }
        }


        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDraftDTO updateDraftDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Guid userId = new Guid(User.FindFirst("UserId").Value);

            try
            {
                ServiceResponse<Articles?> response = await _articleService.UpdateAsync(userId, id, updateDraftDTO);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}\n{ex.InnerException}");
            }
        }


        [Authorize]
        [HttpPost("{id:guid}/review")]
        public async Task<IActionResult> Review(Guid id)
        {
            Guid userId = new Guid(User.FindFirst("UserId").Value);

            try
            {
                ServiceResponse<Articles?> response = await _articleService.MarkAsReviewAsync(userId, id);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}\n{ex.InnerException}");
            }
        }


        [Authorize]
        [HttpPost("{id:guid}/publish")]
        public async Task<IActionResult> Publicate(Guid id)
        {
            Guid userId = new Guid(User.FindFirst("UserId").Value);

            try
            {
                ServiceResponse<Articles?> response = await _articleService.PublicateDraft(userId, id);

                if (!response.IsSuccess)
                    return StatusCode(statusCode: response.Status, new { message = response.Message });

                return Ok(new { data = response.Data, message = response.Message });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: $"Content Service: {ex.Message}\n{ex.InnerException}");
            }
        }
    }
}
