namespace Keeper_ContentService.Models.DTO
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public CategoryDTO Category { get; set; } = null!;
        public ProfileDTO Author { get; set; } = null!;
        public ArticleStatusDTO Status { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsLiked { get; set; } = false;
        public bool IsSaved { get; set; } = false;
        public DateTime? PublicationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
