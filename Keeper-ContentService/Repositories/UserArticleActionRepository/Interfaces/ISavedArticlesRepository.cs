using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces
{
    public interface ISavedArticlesRepository : 
        IUserArticleActionRepository<SavedArticle, SavedArticlesFillterDTO>
    { }
}
