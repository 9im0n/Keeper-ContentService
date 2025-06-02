using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.ArticleRepository.Interfaces;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Keeper_ContentService.Services.ProfileService.Interfaces;
using Keeper_ContentService.Services.UserArticleActionService.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Keeper_ContentService.Services.UserArticleActionService.Implementations
{
    public class LikedArticleService : ILikedArticleService
    {
        private readonly IArticleService _articleService;
        private readonly ILikedArticlesRepository _repository;
        private readonly ISavedArticlesRepository _savedArticlesRepository;
        private readonly IProfileService _profileService;
        private readonly IArticlesStatusesService _articlesStatusesService;
        private readonly IDTOMapperService _mapper;

        public LikedArticleService(
            IArticleService articleService,
            ILikedArticlesRepository repository,
            ISavedArticlesRepository savedArticlesRepository,
            IProfileService profileService,
            IArticlesStatusesService articlesStatusesService,
            IDTOMapperService mapper)
        {
            _articleService = articleService;
            _repository = repository;
            _savedArticlesRepository = savedArticlesRepository;
            _profileService = profileService;
            _articlesStatusesService = articlesStatusesService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PagedResultDTO<ArticleDTO>?>>
            GetPaginationAsync(PagedRequestDTO<LikedArticlesFillterDTO> request, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, 401, "User unauthorized");

            if (request.Filter?.UserId != userId && User.FindFirst(ClaimTypes.Role)?.Value == "user")
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, 403, "You don't have required permissions.");

            PagedResultDTO<LikedArticle> likedArticles = await _repository.GetPagedAsync(request);

            BatchedProfileRequestDTO profileRequest = new BatchedProfileRequestDTO()
            {
                profileIds = likedArticles.Items.Select(l => l.UserId).ToList()
            };

            ServiceResponse<ICollection<ProfileDTO>?> profileResponse = await _profileService
                .GetProfilesBatchAsync(profileRequest);

            if (!profileResponse.IsSuccess)
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, profileResponse.Status, profileResponse.Message);

            ICollection<Article> articles = likedArticles.Items.Select(l => l.Article).ToList();
            ICollection<Guid> articleIds = articles.Select(a => a.Id).ToList();

            ICollection<SavedArticle> savedArticles = await _savedArticlesRepository
                .GetBatchedByUserAndArticleId(articleIds, userId);

            ICollection<ArticleDTO> articleDTOs = _mapper.Map(articles, likedArticles.Items, savedArticles, profileResponse.Data!);

            PagedResultDTO<ArticleDTO> response = new PagedResultDTO<ArticleDTO>()
            {
                Items = articleDTOs.ToList(),
                TotalCount = likedArticles.TotalCount,
            };

            return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Success(response);
        }


        public async Task<ServiceResponse<ArticleDTO?>> AddAsync(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<ArticleDTO?>.Fail(default, 401, "User unauthorized");

            LikedArticle? liked = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (liked != null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 409, "You liked it before.");

            ServiceResponse<ArticleDTO?> article = await _articleService.GetByIdAsync(articleId, User);

            if (!article.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, article.Status, article.Message);

            ServiceResponse<ArticleStatusDTO?> statusResponse = await _articlesStatusesService.GetPublishedStatusAsync();

            if (!statusResponse.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, statusResponse.Status, statusResponse.Message);

            if (article.Data!.Status.Id != statusResponse.Data!.Id)
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "Article doesn't exist.");

            LikedArticle newLikedarticle = new LikedArticle()
            {
                ArticleId = articleId,
                UserId = userId,
            };

            newLikedarticle = await _repository.CreateAsync(newLikedarticle);

            article.Data!.IsLiked = true;

            return ServiceResponse<ArticleDTO?>.Success(article.Data);
        }


        public async Task<ServiceResponse<ArticleDTO?>> RemoveAsync(Guid articleId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<ArticleDTO?>.Fail(default, 401, "User unauthorized");

            LikedArticle? liked = await _repository.GetByUserAndArticleIdAsync(userId, articleId);

            if (liked == null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "Liked article doesn't exist.");

            await _repository.DeleteAsync(liked.Id);

            ServiceResponse<ArticleDTO?> article = await _articleService.GetByIdAsync(liked.ArticleId, User);

            if (!article.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, article.Status, article.Message);

            return ServiceResponse<ArticleDTO?>.Success(article.Data);
        }
    }
}
