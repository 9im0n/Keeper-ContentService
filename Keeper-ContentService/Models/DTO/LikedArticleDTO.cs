namespace Keeper_ContentService.Models.DTO
{
    public class LikedArticleDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
    }
}
