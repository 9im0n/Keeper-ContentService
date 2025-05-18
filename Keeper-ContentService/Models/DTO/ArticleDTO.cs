namespace Keeper_ContentService.Models.DTO
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public CategoryDTO Category { get; set; } = null!;
        public ProfileDTO Profile { get; set; } = null!;
        public ArticleStatusDTO Satus { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsLiked { get; set; } = false;
        public bool IsSaved { get; set; } = false;
        public DateTime? PublicationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<CommentDTO> Comments { get; set; } = null!;
    }
}
