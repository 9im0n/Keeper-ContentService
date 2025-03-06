using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    public class Favorites : BaseModel
    {
        [Required]
        public Guid ArticleId { get; set; }

        [JsonIgnore]
        public virtual Articles Article { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
