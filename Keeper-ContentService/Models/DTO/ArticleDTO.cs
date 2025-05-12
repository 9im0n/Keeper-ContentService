namespace Keeper_ContentService.Models.DTO
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public CategoryDTO Category { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public ArticleStatusDTO Satus { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public ICollection<CommentDTO> Comments { get; set; } = null!;
    }
}
