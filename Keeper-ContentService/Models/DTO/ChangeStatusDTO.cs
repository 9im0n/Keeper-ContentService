using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.DTO
{
    public class ChangeStatusDTO
    {
        [Required]
        public string Status { get; set; } = null!;
    }
}
