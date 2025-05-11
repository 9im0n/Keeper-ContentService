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


        public async Task<ServiceResponse<ArticleStatus?>> GetByIdAsync(Guid Id)
        {
            ArticleStatus? statuse = await _repository.GetByIdAsync(Id);

            if (statuse == null)
                return ServiceResponse<ArticleStatus?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatus?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatus?>> GetReviewStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("review");

            if (statuse == null)
                return ServiceResponse<ArticleStatus?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatus?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatus?>> GetDraftStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("draft");

            if (statuse == null)
                return ServiceResponse<ArticleStatus?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatus?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatus?>> GetPublishedStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("published");

            if (statuse == null)
                return ServiceResponse<ArticleStatus?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatus?>.Success(statuse);
        }


        public async Task<ServiceResponse<ArticleStatus?>> GetReadyForPublisStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("readyForPublish");

            if (statuse == null)
                return ServiceResponse<ArticleStatus?>.Fail(default, 404, "Statuse doesn't exist");

            return ServiceResponse<ArticleStatus?>.Success(statuse);
        }
    }
}
