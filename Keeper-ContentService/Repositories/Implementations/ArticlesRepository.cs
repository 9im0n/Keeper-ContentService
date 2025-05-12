using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class ArticlesRepository : BaseRepository<Article>, IArticlesRepository
    {
        private IDTOMapperService _mapper;

        public ArticlesRepository(AppDbContext context, IDTOMapperService mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<ArticleDTO>> GetPagedArticlesAsync(PagedRequestDTO<ArticlesFillterDTO> request)
        {
            IQueryable<Article> query = _appDbContext.Articles;

            query = query.Include(a => a.Category).Include(a => a.Status);

            if (!string.IsNullOrEmpty(request.Filter?.Category))
                query = query.Where(a => a.Category.Name == request.Filter.Category);

            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(a => a.Title.Contains(request.Search));

            int totalCount = await query.CountAsync();

            bool isDescending = request.Direction.ToLower() == "desc";

            query = request.Sort.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id),
                "title" => isDescending ? query.OrderByDescending(a => a.Title) : query.OrderBy(a => a.Title),
                "publicationdate" => isDescending ? query.OrderByDescending(a => a.PublicationDate) : query.OrderBy(a => a.PublicationDate),
                _ => isDescending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id)
            };

            query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            List<ArticleDTO> articlesDTOs = _mapper.Map(await query.ToListAsync()).ToList();

            return new PagedResultDTO<ArticleDTO>()
            {
                Items = articlesDTOs,
                TotalCount = totalCount
            };
        }

        public async override Task<Article?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Articles.Include(a => a.Category)
                .Include(a => a.Status).FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
