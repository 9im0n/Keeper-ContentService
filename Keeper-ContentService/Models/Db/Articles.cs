using System.ComponentModel.DataAnnotations;
using Keeper_ContentService.Enums;

namespace Keeper_ContentService.Models.Db
{
    public class Articles : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(7000)]
        public string Content { get; set; }

        [Required]
        public ArticleStatus Status { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
