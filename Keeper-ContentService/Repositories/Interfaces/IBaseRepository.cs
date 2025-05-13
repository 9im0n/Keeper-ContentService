using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        public Task<ICollection<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(Guid Id);
        public Task<T> CreateAsync(T entity);
        public Task<T?> UpdateAsync(T entity);
        public Task<T?> DeleteAsync(Guid Id);
    }
}
