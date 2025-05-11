using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper_ContentService.Models.Db
{
    [Table("ArticleStatuses")]
    public class ArticleStatus : BaseModel
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
