using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(AppDbContext context) : base(context) { }

        public async Task<ICollection<Comment>> GetByUserId(Guid userId)
        {
            return await _appDbContext.Comments.Where(c => c.AuthorId == userId).ToListAsync();
        }


        public async Task<ICollection<Comment>> GetByArticleId(Guid articleId)
        {
            return await _appDbContext.Comments.Where(c => c.ArticleId == articleId).ToListAsync();
        }
    }
}
