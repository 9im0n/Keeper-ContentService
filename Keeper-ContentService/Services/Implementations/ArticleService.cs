using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticlesRepository _articlesRepository;
        private readonly IArticlesStatusesService _articlesStatusesService;
        private readonly IDTOMapperService _mapper;
        private readonly ICategoriesService _categoryService;

        public ArticleService(IArticlesRepository articlesRepository,
            IArticlesStatusesService articlesStatusesService,
            IDTOMapperService mapper,
            ICategoriesService categoryService)
        {
            _articlesRepository = articlesRepository;
            _articlesStatusesService = articlesStatusesService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public async Task<ServiceResponse<PagedResultDTO<ArticleDTO>>> GetPagedAsync(
            PagedRequestDTO<ArticlesFillterDTO> pagedRequestDTO)
        {
            PagedResultDTO<ArticleDTO> pagedResultDTO = await _articlesRepository.GetPagedArticlesAsync(pagedRequestDTO);
            return ServiceResponse<PagedResultDTO<ArticleDTO>>.Success(pagedResultDTO);
        }

        public async Task<ServiceResponse<ArticleDTO?>> GetById(Guid id)
        {
            Article? article = await _articlesRepository.GetByIdAsync(id);

            if (article == null)
                return ServiceResponse<ArticleDTO?>.Fail(default, 404, "Article doesn't exist.");

            ArticleDTO articleDTO = _mapper.Map(article);
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
                ArticleStatusId = statuseServiceResponse.Data.Id,
                Content = createDraftDTO.Content,
                CategoryId = categoryServiceResponse.Data.Id
            };

            newArticle = await _articlesRepository.CreateAsync(newArticle);

            ArticleDTO articleDTO = _mapper.Map(newArticle);

            return ServiceResponse<ArticleDTO?>.Success(articleDTO, 201);
        }
    }
}
