using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.ArticleStatusService.Interfaces
{
    public interface IArticlesStatusesService : IArticleStatusReaderService, IArticleStatusNamedProvider { }
}
