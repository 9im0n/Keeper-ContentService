using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.DTO
{
    public class UpdateArticleDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public CategoryDTO Category { get; set; } = null!;

        [Required]
        public ArticleStatusDTO Status { get; set; } = null!;

        [Required]
        [MaxLength(10000)]
        public string Content { get; set; } = null!;
    }
}
