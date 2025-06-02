namespace Keeper_ContentService.Models.DTO
{
    public class ArticlesFillterDTO
    {
        public string? Category { get; set; }
        public Guid? UserId { get; set; }
        public string? StatusName { get; set; }
    }
}
