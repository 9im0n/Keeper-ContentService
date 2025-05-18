namespace Keeper_ContentService.Models.DTO
{
    public class PagedResponseDTO<T>
    {
        public ICollection<T> Items { get; set; } = null!;
        public int TotalCount { get; set; }
    }
}
