using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.DTO
{
    public class UpdateDraftDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(7000)]
        public string Content { get; set; }
    }
}
