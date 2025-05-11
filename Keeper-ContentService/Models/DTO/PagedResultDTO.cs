namespace Keeper_ContentService.Models.DTO
{
    public class PagedResultDTO<TData>
    {
        public int TotalCount { get; set; }
        public List<TData> Items { get; set; } = null!;
    }
}
