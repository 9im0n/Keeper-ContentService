using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class LikedArticlesRepository : BaseRepository<LikedArticle>, ILikedArticlesRepository
    {
        private readonly IDTOMapperService _mapper;

        public LikedArticlesRepository(AppDbContext context, IDTOMapperService mapper): base(context)
        {
            _mapper = mapper;
        }

        
        public async Task<PagedResultDTO<LikedArticleDTO>> 
            GetPagedLikedArticlesAsync(PagedRequestDTO<LikedArticlesFillterDTO> request)
        {
            IQueryable<LikedArticle> query = _appDbContext.Likes;

            if (request.Filter?.UserId != null)
                query = query.Where(l => l.UserId == request.Filter.UserId);

            int totalCount = await query.CountAsync();

            bool isDescending = request.Direction.ToLower() == "desc";

            query = request.Sort.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(l => l.Id) : query.OrderBy(l => l.Id),
                "createdat" => isDescending ? query.OrderByDescending(l => l.CreatedAt) : query.OrderBy(l => l.CreatedAt),
                _ => isDescending ? query.OrderByDescending(l => l.Id) : query.OrderBy(l => l.Id)
            };

            query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            List<LikedArticleDTO> likedArticleDTOs = _mapper.Map(await query.ToListAsync()).ToList();

            return new PagedResultDTO<LikedArticleDTO>()
            {
                Items = likedArticleDTOs,
                TotalCount = totalCount
            };
        }
    }
}
