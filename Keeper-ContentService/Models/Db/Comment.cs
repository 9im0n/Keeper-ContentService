using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    [Table("Comments")]
    public class Comment : BaseModel
    {
        public string Text = null!;

        public Article Article { get; set; } = null!;

        public Comment? ParentComment { get; set; }

        public Guid AuthorId { get; set; }

        public Guid ArticleId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
