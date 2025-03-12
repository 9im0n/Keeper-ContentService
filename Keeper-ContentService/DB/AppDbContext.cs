using Keeper_ContentService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<Articles> Articles { get; set; } = null!;
        public DbSet<ArticleStatuses> ArticleStatuses { get; set; } = null!;
        public DbSet<Comments> Comments { get; set; } = null!;
        public DbSet<Categories> Categories { get; set; } = null!;
        public DbSet<LikedArticles> Likes { get; set; } = null!;
        public DbSet<Favorites> Favorites { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
