﻿using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.DTO
{
    public class CreateDraftDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Content { get; set; } = null!;
    }
}
