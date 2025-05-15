using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.BaseRepository.Interfaces
{
    public interface IDeletableRepository<T> where T : BaseModel
    {
        Task<T?> DeleteAsync(Guid id);
    }
}
