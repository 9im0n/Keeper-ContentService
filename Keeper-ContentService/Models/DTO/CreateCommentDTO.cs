using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.DTO
{
    public class CreateCommentDTO
    {
        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = null!;

        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        public Guid? ParentCommentId { get; set; }
    }
}
