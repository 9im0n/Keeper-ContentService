using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class SavedArticlesRepository : BaseRepository<SavedArticle>, ISavedArticlesRepository
    {
        private readonly IDTOMapperService _mapper;

        public SavedArticlesRepository(AppDbContext context,
            IDTOMapperService mapper): base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<SavedArticleDTO>>
            GetPagedLikedArticlesAsync(PagedRequestDTO<SavedArticlesFillterDTO> request)
        {
            IQueryable<SavedArticle> query = _appDbContext.Favorites;

            if (request.Filter?.UserId != null)
                query = query.Where(s => s.UserId == request.Filter.UserId);

            int totalCount = await query.CountAsync();

            bool isDescending = request.Direction.ToLower() == "desc";

            query = request.Sort.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id),
                "createdat" => isDescending ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
                _ => isDescending ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
            };

            query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            List<SavedArticleDTO> savedArticleDTOs = _mapper.Map(await query.ToListAsync()).ToList();

            return new PagedResultDTO<SavedArticleDTO>()
            {
                Items = savedArticleDTOs,
                TotalCount = totalCount
            };
        }


        public async Task<SavedArticle?> GetByUserAndArticleIdAsync(Guid userId, Guid articleId)
        {
            return await _appDbContext.Favorites.FirstOrDefaultAsync(s => s.UserId == userId && s.ArticleId == articleId);
        }
    }
}
