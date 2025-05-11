using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    [Table("LikedArticles")]
    public class LikedArticle : BaseModel
    {
        public Article Article { get; set; } = null!;

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ArticleId { get; set; }
    }
}
