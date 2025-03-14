using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;

namespace Keeper_ContentService.Services.Implementations
{
    public class ArticleStatusesService : IArticlesStatusesService
    {
        private readonly IArticleStatusesRepository _repository;

        public ArticleStatusesService(IArticleStatusesRepository repository)
        {
            _repository = repository;
        }


        public async Task<ServiceResponse<ArticleStatuses?>> GetByIdAsync(Guid Id)
        {
            ArticleStatuses? statuse = await _repository.GetByIdAsync(Id);

            if (statuse == null)
                return ServiceResponse<ArticleStatuses?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatuses?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatuses?>> GetReviewStatusAsync()
        {
            ArticleStatuses? statuse = await _repository.GetByNameAsync("review");

            if (statuse == null)
                return ServiceResponse<ArticleStatuses?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatuses?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatuses?>> GetDraftStatusAsync()
        {
            ArticleStatuses? statuse = await _repository.GetByNameAsync("draft");

            if (statuse == null)
                return ServiceResponse<ArticleStatuses?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatuses?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatuses?>> GetPublishedStatusAsync()
        {
            ArticleStatuses? statuse = await _repository.GetByNameAsync("published");

            if (statuse == null)
                return ServiceResponse<ArticleStatuses?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatuses?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatuses?>> GetReadyForPublisStatusAsync()
        {
            ArticleStatuses? statuse = await _repository.GetByNameAsync("readyForPublish");

            if (statuse == null)
                return ServiceResponse<ArticleStatuses?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatuses?>.Success(statuse);
        }
    }
}
