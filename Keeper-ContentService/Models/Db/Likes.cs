using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Keeper_ContentService.Models.Db
{
    public class Likes : BaseModel
    {
        [Required]
        public Guid ArticlesId { get; set; }

        [JsonIgnore]
        public virtual Articles Article { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
