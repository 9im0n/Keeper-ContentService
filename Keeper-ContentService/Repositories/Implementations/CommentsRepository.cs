using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        private readonly IDTOMapperService _mapper;

        public CommentsRepository(AppDbContext context, IDTOMapperService mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<CommentDTO>> GetPagedCommentsAsync(Guid articleId, PagedRequestDTO<CommentsFilterDTO> request)
        {
            IQueryable<Comment> query = _appDbContext.Comments.Where(c => c.ArticleId == articleId);

            if (request.Filter?.AuthorId != null)
                query = query.Where(c => c.AuthorId == request.Filter.AuthorId);

            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(c => c.Text.Contains(request.Search));

            int totalCount = await query.CountAsync();

            bool isDescending = request.Direction.ToLower() == "desc";

            query = request.Sort.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                "createdAt" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                _ => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id)
            };

            query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            List<CommentDTO> commentDTOs = _mapper.Map(await query.ToListAsync()).ToList();

            return new PagedResultDTO<CommentDTO>()
            {
                TotalCount = totalCount,
                Items = commentDTOs,
            };
        }
    }
}
