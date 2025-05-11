using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper_ContentService.Models.Db
{
    [Table("Categories")]
    public class Category : BaseModel
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
