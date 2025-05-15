using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.CommentService.Interfaces
{
    public interface ICommentWriterService
    {
        Task<ServiceResponse<CommentDTO?>> CreateAsync(
            Guid articleId,
            CreateCommentDTO createCommentDTO,
            ClaimsPrincipal user);

        Task<ServiceResponse<object?>> DeleteAsync(
            Guid articleId,
            Guid commentId,
            ClaimsPrincipal user);
    }
}
