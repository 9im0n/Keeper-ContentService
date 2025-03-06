using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    public class Comments : BaseModel
    {
        [Required]
        public Guid ArticleId { get; set; }

        [JsonIgnore]
        public virtual Articles Article { get; set; }

        public Guid? PerantId { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
