using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.BaseRepository.Interfaces
{
    public interface IReadableRepository<T> where T : BaseModel
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
    }
}
