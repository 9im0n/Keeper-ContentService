namespace Keeper_ContentService.Models.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid ArticleId { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
