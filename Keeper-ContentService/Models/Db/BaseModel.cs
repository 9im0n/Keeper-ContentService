using System.ComponentModel.DataAnnotations;

namespace Keeper_ContentService.Models.Db
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
    }
}
