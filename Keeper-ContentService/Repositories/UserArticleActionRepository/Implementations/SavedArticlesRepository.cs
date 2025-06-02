using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.BaseRepository.Implementations;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.UserArticleActionRepository.Implementations
{
    public class SavedArticlesRepository : 
        BaseRepository<SavedArticle>,
        ISavedArticlesRepository
    {
        public SavedArticlesRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResultDTO<SavedArticle>>
            GetPagedAsync(PagedRequestDTO<SavedArticlesFillterDTO> request)
        {
            IQueryable<SavedArticle> query = _appDbContext.Favorites;

            query = query.Include(l => l.Article).ThenInclude(a => a.Status)
                .Include(l => l.Article).ThenInclude(a => a.Category);

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

            List<SavedArticle> savedArticles = await query.ToListAsync();

            return new PagedResultDTO<SavedArticle>()
            {
                Items = savedArticles,
                TotalCount = totalCount
            };
        }


        public async Task<SavedArticle?> GetByUserAndArticleIdAsync(Guid userId, Guid articleId)
        {
            return await _appDbContext.Favorites.FirstOrDefaultAsync(s => s.UserId == userId && s.ArticleId == articleId);
        }


        public async Task<ICollection<SavedArticle>> GetBatchedByUserAndArticleId(ICollection<Guid> articleId, Guid userId)
        {
            return await _appDbContext.Favorites.Where(s => articleId.Contains(s.ArticleId) && s.UserId == userId).ToListAsync();
        }
    }
}
