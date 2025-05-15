using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Services.UserArticleActionService.Interfaces
{
    public interface ILikedArticleService :
        IUserArticleActionReaderService<LikedArticleDTO, LikedArticlesFillterDTO>,
        IUserArticleActionWriterService
    { }
}
