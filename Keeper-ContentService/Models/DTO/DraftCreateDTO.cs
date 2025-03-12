using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Models.DTO
{
    public class DraftCreateDTO
    {
        public string Title { get; set; }

        public Guid CategoryId { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }

        public Guid StatuseId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
