using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.CommentService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper_ContentService.Controllers
{
    [ApiController]
    [Route("articles/{articleId:guid}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentsService;

        public CommentsController(ICommentService commentsService)
        {
            _commentsService = commentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedComments(Guid articleId, 
            [FromQuery] PagedRequestDTO<CommentsFilterDTO> pagedRequestDTO)
        {
            ServiceResponse<PagedResultDTO<CommentDTO>?> response = await _commentsService
                .GetPagedAsync(articleId, pagedRequestDTO);
            return HandleServiceResponse(response);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(Guid articleId, 
            [FromBody] CreateCommentDTO createCommentDTO)
        {
            ServiceResponse<CommentDTO?> response = await _commentsService.CreateAsync(articleId, 
                createCommentDTO, User);
            return HandleServiceResponse(response);
        }


        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteComment(Guid articleId, Guid id)
        {
            ServiceResponse<object?> response = await _commentsService.DeleteAsync(articleId, id, User);
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
