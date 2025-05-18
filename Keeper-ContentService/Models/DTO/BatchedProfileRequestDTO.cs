namespace Keeper_ContentService.Models.DTO
{
    public class BatchedProfileRequestDTO
    {
        public ICollection<Guid> profileIds { get; set; } = null!;
    }
}
