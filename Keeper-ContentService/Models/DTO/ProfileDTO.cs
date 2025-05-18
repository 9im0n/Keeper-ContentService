namespace Keeper_ContentService.Models.DTO
{
    public class ProfileDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
    }
}
