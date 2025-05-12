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
        private readonly IDTOMapperService _mapper;

        public ArticleService(IArticlesRepository articlesRepository,
            IArticlesStatusesService articlesStatusesService,
            IDTOMapperService mapper)
        {
            _articlesRepository = articlesRepository;
            _articlesStatusesService = articlesStatusesService;
            _mapper = mapper;
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
    }
}
