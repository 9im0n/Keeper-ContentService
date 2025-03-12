using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.Db
{
    public class Articles : BaseModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public Categories Category { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(7000)]
        public string Content { get; set; }

        [Required]
        public ArticleStatuses Statuse { get; set; }

        [Required]
        public Guid StatuseId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
