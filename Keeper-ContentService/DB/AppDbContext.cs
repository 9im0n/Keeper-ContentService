using Keeper_ContentService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<ArticleStatus> ArticleStatuses { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<LikedArticle> Likes { get; set; } = null!;
        public DbSet<SavedArticle> Favorites { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
