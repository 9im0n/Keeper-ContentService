using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper_ContentService.Models.Db
{
    public class ArticleStatuses : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
