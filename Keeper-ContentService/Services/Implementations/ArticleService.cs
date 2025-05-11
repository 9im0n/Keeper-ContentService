using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;

namespace Keeper_ContentService.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticlesRepository _articlesRepository;
        private readonly IArticlesStatusesService _articlesStatusesService;

        public ArticleService(IArticlesRepository articlesRepository,
            IArticlesStatusesService articlesStatusesService)
        {
            _articlesRepository = articlesRepository;
            _articlesStatusesService = articlesStatusesService;
        }
    }
}
