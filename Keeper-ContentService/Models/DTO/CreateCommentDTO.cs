using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.DTO
{
    public class CreateCommentDTO
    {
        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = null!;

        public Guid? ParentCommentId { get; set; } = null;
    }
}
