using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Implementations;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Keeper_ContentService.Services.ProfileService.Interfaces;
using Keeper_ContentService.Services.UserArticleActionService.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.UserArticleActionService.Implementations
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly ISavedArticlesRepository _repository;
        private readonly ILikedArticlesRepository _likedArticlesRepository;
        private readonly IArticleService _articleService;
        private readonly IProfileService _profileService;
        private readonly IArticlesStatusesService _articlesStatusesService;
        private readonly IDTOMapperService _mapper;

        public SavedArticleService(
            ISavedArticlesRepository repository,
            ILikedArticlesRepository likedArticlesRepository,
            IArticleService articleService,
            IProfileService profileService,
            IArticlesStatusesService articlesStatusesService,
            IDTOMapperService mapper)
        {
            _repository = repository;
            _likedArticlesRepository = likedArticlesRepository;
            _articleService = articleService;
            _profileService = profileService;
            _articlesStatusesService = articlesStatusesService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PagedResultDTO<ArticleDTO>?>>
            GetPaginationAsync(PagedRequestDTO<SavedArticlesFillterDTO> request, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, 401, "User unauthorized");

            if (request.Filter?.UserId != userId && User.FindFirst(ClaimTypes.Role)?.Value == "user")
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, 403, "You don't have required permissions.");

            PagedResultDTO<SavedArticle> savedArticles = await _repository.GetPagedAsync(request);

            BatchedProfileRequestDTO profileRequest = new BatchedProfileRequestDTO()
            {
                profileIds = savedArticles.Items.Select(l => l.UserId).ToList()
            };

            ServiceResponse<ICollection<ProfileDTO>?> profileResponse = await _profileService
                .GetProfilesBatchAsync(profileRequest);

            if (!profileResponse.IsSuccess)
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, profileResponse.Status, profileResponse.Message);

            ICollection<Article> articles = savedArticles.Items.Select(l => l.Article).ToList();
            ICollection<Guid> articleIds = articles.Select(a => a.Id).ToList();

            ICollection<LikedArticle> likedArticles = await _likedArticlesRepository
                .GetBatchedByUserAndArticleId(articleIds, userId);

            ICollection<ArticleDTO> articleDTOs = _mapper.Map(articles, likedArticles, savedArticles.Items, profileResponse.Data!);

            PagedResultDTO<ArticleDTO> response = new PagedResultDTO<ArticleDTO>()
            {
                Items = articleDTOs.ToList(),
                TotalCount = savedArticles.TotalCount,
            };

            return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Success(response);
        }


        public async Task<ServiceResponse<ArticleDTO?>> AddAsync(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<ArticleDTO?>.Fail(default, 401, "User unauthorized");

            SavedArticle? savedArticle = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (savedArticle != null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 409, "You already saved it.");

            ServiceResponse<ArticleDTO?> article = await _articleService.GetByIdAsync(articleId);

            if (!article.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, article.Status, article.Message);

            ServiceResponse<ArticleStatusDTO?> statusResponse = await _articlesStatusesService.GetPublishedStatusAsync();

            if (!statusResponse.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, statusResponse.Status, statusResponse.Message);

            savedArticle = new SavedArticle()
            {
                ArticleId = articleId,
                UserId = userId,
            };

            savedArticle = await _repository.CreateAsync(savedArticle);

            article.Data!.IsSaved = true;

            return ServiceResponse<ArticleDTO?>.Success(article.Data);
        }


        public async Task<ServiceResponse<ArticleDTO?>> RemoveAsync(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<ArticleDTO?>.Fail(default, 401, "User unauthorized");

            SavedArticle? savedArticle = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (savedArticle == null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "You didn't save this article.");

            savedArticle = await _repository.DeleteAsync(savedArticle.Id);

            ServiceResponse<ArticleDTO?> article = await _articleService.GetByIdAsync(savedArticle!.ArticleId, User);

            if (!article.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, article.Status, article.Message);

            return ServiceResponse<ArticleDTO?>.Success(article.Data);
        }
    }
}
