using Keeper_ContentService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.DB
{
    public class AppDbContext : DbContext
    {
        DbSet<Articles> Articles { get; set; } = null!;
        DbSet<Categories> Categories { get; set; } = null!;
        DbSet<Comments> Comments { get; set; } = null!;
        DbSet<Likes> Likes { get; set; } = null!;
        DbSet<Favorites> Favorites { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
