using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;

namespace Keeper_ContentService.Services.CommentService.Interfaces
{
    public interface ICommentService : ICommentReaderService, ICommentWriterService { }
}