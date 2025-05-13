using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    [Table("Comments")]
    public class Comment : BaseModel
    {
        public string Text { get; set; } = null!;

        public Article Article { get; set; } = null!;

        public Comment? ParentComment { get; set; }

        public Guid AuthorId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ArticleId { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
