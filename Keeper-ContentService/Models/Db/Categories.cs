using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.Db
{
    public class Categories : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
