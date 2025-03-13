using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    public class Articles : BaseModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public virtual Categories Category { get; set; }

        [Required]
        [JsonIgnore]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(7000)]
        public string Content { get; set; }

        [Required]
        public virtual ArticleStatuses Statuse { get; set; }

        [Required]
        [JsonIgnore]
        public Guid StatuseId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
