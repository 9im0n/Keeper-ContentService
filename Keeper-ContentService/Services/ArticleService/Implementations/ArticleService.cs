using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.ArticleRepository.Interfaces;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using Keeper_ContentService.Services.CategoryService.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Keeper_ContentService.Services.ProfileService.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.ArticleService.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticlesRepository _articlesRepository;
        private readonly IArticlesStatusesService _articlesStatusesService;
        private readonly IDTOMapperService _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IProfileService _profileService;
        private readonly ILikedArticlesRepository _likedArticlesRepository;
        private readonly ISavedArticlesRepository _savedArticlesRepository;
        private readonly IEnumerable<IStatusChangeStrategy> _statusChangeStrategies;


        public ArticleService(
            IArticlesRepository articlesRepository,
            IArticlesStatusesService articlesStatusesService,
            IDTOMapperService mapper,
            ICategoryService categoryService,
            IProfileService profileService,
            ILikedArticlesRepository likedArticlesRepository,
            ISavedArticlesRepository savedArticlesRepository,
            IEnumerable<IStatusChangeStrategy> statusChangeStrategies
        )
        {
            _articlesRepository = articlesRepository;
            _articlesStatusesService = articlesStatusesService;
            _mapper = mapper;
            _categoryService = categoryService;
            _profileService = profileService;
            _likedArticlesRepository = likedArticlesRepository;
            _savedArticlesRepository = savedArticlesRepository;
            _statusChangeStrategies = statusChangeStrategies;
        }


        public async Task<ServiceResponse<PagedResultDTO<ArticleDTO>?>> GetPagedAsync(
            PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO,
            ClaimsPrincipal? User = null)
        {
            ServiceResponse<ArticleStatusDTO?> statusResponse = await _articlesStatusesService.GetPublishedStatusAsync();

            if (!statusResponse.IsSuccess)
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, statusResponse.Status, statusResponse.Message);

            Guid? userId = Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid parsedId)
                ? parsedId : null;

            Console.WriteLine(pagedRequestDTO.Filter?.StatusName);

            string userRole = User?.FindFirst(ClaimTypes.Role)?.Value ?? "user";

            if (pagedRequestDTO.Filter?.UserId != userId &&
                pagedRequestDTO.Filter?.StatusName != statusResponse.Data!.Name &&
                userRole == "user")
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, 403, "You don't have required permissions");


            PagedResponseDTO<Article> pagedResponseDTO = await _articlesRepository.GetPagedArticlesAsync(pagedRequestDTO);
            
            BatchedProfileRequestDTO profileIds = new BatchedProfileRequestDTO() 
            { 
                profileIds = pagedResponseDTO.Items.Select(a => a.AuthorId).Distinct().ToList() 
            };

            ServiceResponse<ICollection<ProfileDTO>?> profileDTOs = await _profileService.GetProfilesBatchAsync(profileIds);

            if (!profileDTOs.IsSuccess)
                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Fail(default, profileDTOs.Status, profileDTOs.Message);

            ICollection<ArticleDTO> articleDTOs;
            PagedResultDTO<ArticleDTO> response;

            if (userId != null)
            {
                ICollection<Guid> articleIds = pagedResponseDTO.Items.Select(a => a.Id).ToList();
                
                ICollection<LikedArticle> likedArticles = await _likedArticlesRepository
                    .GetBatchedByUserAndArticleId(articleIds, userId.Value);
                ICollection<SavedArticle> savedArticles = await _savedArticlesRepository
                    .GetBatchedByUserAndArticleId(articleIds, userId.Value);

                articleDTOs = _mapper.Map(pagedResponseDTO.Items, likedArticles, savedArticles, profileDTOs.Data!);

                response = new PagedResultDTO<ArticleDTO>()
                {
                    Items = articleDTOs.ToList(),
                    TotalCount = pagedResponseDTO.TotalCount
                };

                return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Success(response);
            }

            articleDTOs = _mapper.Map(pagedResponseDTO.Items, null, null, profileDTOs.Data!);

            response = new PagedResultDTO<ArticleDTO>()
            {
                Items = articleDTOs.ToList(),
                TotalCount = pagedResponseDTO.TotalCount
            };

            return ServiceResponse<PagedResultDTO<ArticleDTO>?>.Success(response);
        }


        public async Task<ServiceResponse<ArticleDTO?>> GetByIdAsync(Guid id, ClaimsPrincipal? User)
        {
            ServiceResponse<ArticleStatusDTO?> statusResponse = await _articlesStatusesService.GetPublishedStatusAsync();

            if (!statusResponse.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, statusResponse.Status, statusResponse.Message);

            Guid? userId = Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid parsedId)
                ? parsedId : null;

            string userRole = User?.FindFirst(ClaimTypes.Role)?.Value ?? "user";

            Article? article = await _articlesRepository.GetByIdAsync(id);

            if (article == null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "Article doesn't exist.");

            if (article.ArticleStatusId != statusResponse.Data!.Id &&
                article.AuthorId != userId && userRole == "user")
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "Article doesn't exist.");

            bool isLiked = false;
            bool isSaved = false;

            if (userId != null)
            {
                isLiked = (await _likedArticlesRepository.GetByUserAndArticleIdAsync(userId.Value, article.Id)) == null ? false : true;
                isSaved = (await _savedArticlesRepository.GetByUserAndArticleIdAsync(userId.Value, article.Id)) == null ? false : true;
            }

            ServiceResponse<ProfileDTO?> profileServiceResponse = await _profileService.GetProfileByIdAsync(article.AuthorId);

            if (!profileServiceResponse.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, profileServiceResponse.Status, profileServiceResponse.Message);

            ArticleDTO articleDTO = _mapper.Map(article, isLiked, isSaved, profileServiceResponse.Data!);
            return ServiceResponse<ArticleDTO?>.Success(articleDTO);
        }


        public async Task<ServiceResponse<ArticleDTO?>> CreateDraftAsync(CreateDraftDTO createDraftDTO,
            ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<ArticleDTO?>.Fail(default, 401, "user unauthorized.");

            ServiceResponse<ArticleStatusDTO?> statuseServiceResponse = await _articlesStatusesService
                .GetDraftStatusAsync();

            if (!statuseServiceResponse.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, statuseServiceResponse.Status,
                    statuseServiceResponse.Message);

            ServiceResponse<CategoryDTO?> categoryServiceResponse = await _categoryService.GetByIdAsync(createDraftDTO.CategoryId);

            if (!categoryServiceResponse.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, categoryServiceResponse.Status,
                    categoryServiceResponse.Message);

            Article? newArticle = new Article()
            {
                Title = createDraftDTO.Title,
                AuthorId = userId,
                ArticleStatusId = statuseServiceResponse.Data!.Id,
                Content = createDraftDTO.Content,
                CategoryId = categoryServiceResponse.Data!.Id
            };

            newArticle = await _articlesRepository.CreateAsync(newArticle);

            ServiceResponse<ArticleDTO?> createdArticle = await GetByIdAsync(newArticle.Id, User);

            if (!createdArticle.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, createdArticle.Status, createdArticle.Message);

            return ServiceResponse<ArticleDTO?>.Success(createdArticle.Data, 201);
        }


        public async Task<ServiceResponse<ArticleDTO?>> UpdateArticleAsync(Guid id,
            UpdateArticleDTO updateArticleDTO,
            ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<ArticleDTO?>.Fail(default, 401, "User Unauthorized.");

            Article? article = await _articlesRepository.GetByIdAsync(id);

            if (article == null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "Article doesn't exist.");

            if (article.AuthorId != userId)
                return ServiceResponse<ArticleDTO?>.Fail(default, 403, "You don't have permission to change this article.");

            ServiceResponse<ArticleStatusDTO?> publishedStatus = await _articlesStatusesService.GetPublishedStatusAsync();

            if (!publishedStatus.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, publishedStatus.Status, publishedStatus.Message);

            if (article.ArticleStatusId == publishedStatus.Data!.Id)
                return ServiceResponse<ArticleDTO?>.Fail(default, 400, "You cannot update published articles.");

            ServiceResponse<ArticleStatusDTO?> draftStatus = await _articlesStatusesService.GetDraftStatusAsync();

            if (!draftStatus.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, draftStatus.Status, draftStatus.Message);

            article.Title = updateArticleDTO.Title;
            article.CategoryId = updateArticleDTO.CategoryId;
            article.Content = updateArticleDTO.Content;
            article.ArticleStatusId = draftStatus.Data!.Id;
            article.UpdatedAt = DateTime.UtcNow;

            await _articlesRepository.UpdateAsync(article);

            ServiceResponse<ArticleDTO?> updatedArticle = await GetByIdAsync(id, User);

            if (!updatedArticle.IsSuccess)
                return ServiceResponse<ArticleDTO?>.Fail(default, updatedArticle.Status, updatedArticle.Message);

            return ServiceResponse<ArticleDTO?>.Success(updatedArticle.Data);
        }


        public async Task<ServiceResponse<object?>> DeleteAsync(Guid id, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<object?>.Fail(default, 401, "User Unauthorized");

            Article? article = await _articlesRepository.GetByIdAsync(id);

            if (article == null)
                return ServiceResponse<object?>.Fail(default, 404, "Article doesn't exist.");

            if (article.AuthorId != userId)
                return ServiceResponse<object?>.Fail(default, 403, "You don't have permission to delete this article.");

            await _articlesRepository.DeleteAsync(id);

            return ServiceResponse<object?>.Success(null);
        }


        public async Task<ServiceResponse<ArticleDTO?>> ChangeStatusAsync(Guid id, ChangeStatusDTO changeStatusDTO, ClaimsPrincipal User)
        {
            Article? article = await _articlesRepository.GetByIdAsync(id);

            if (article == null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "Article doesn't exist.");

            
            foreach (IStatusChangeStrategy strategy in _statusChangeStrategies)
            {
                if (await strategy.CanHandle(changeStatusDTO.Status))
                {
                    ServiceResponse<Article?> updateArticle = await strategy.ChangeStatusAsync(article, User);

                    if (!updateArticle.IsSuccess)   
                        return ServiceResponse<ArticleDTO?>.Fail(default, updateArticle.Status, updateArticle.Message);

                    await _articlesRepository.UpdateAsync(updateArticle.Data!);

                    return await GetByIdAsync(id, User);
                }
            }

            return ServiceResponse<ArticleDTO?>.Fail(default, 400, "It's imposible to process this status.");
        }
    }
}
