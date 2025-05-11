using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    [Table("Articles")]
    public class Article : BaseModel
    {
        public string Title { get; set; } = null!;

        public Category Category { get; set; } = null!;

        public ArticleStatus Status { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime PublicationDate { get; set; }

        public ICollection<Comment> Comments { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ArticleStatusId { get; set; }
    }
}
