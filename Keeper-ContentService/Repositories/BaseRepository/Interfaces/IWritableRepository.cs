using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.BaseRepository.Interfaces
{
    public interface IWritableRepository<T> where T : BaseModel
    {
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(T entity);
    }
}
