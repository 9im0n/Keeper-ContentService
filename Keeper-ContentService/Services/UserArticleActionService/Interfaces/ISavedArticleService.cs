using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Services.UserArticleActionService.Interfaces
{
    public interface ISavedArticleService :
        IUserArticleActionReaderService<ArticleDTO, SavedArticlesFillterDTO>,
        IUserArticleActionWriterService
    { }
}
