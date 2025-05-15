using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Keeper_ContentService.Repositories.BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.BaseRepository.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly AppDbContext _appDbContext;

        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _appDbContext.Set<T>().ToListAsync();
        }


        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Set<T>().FirstOrDefaultAsync(obj => obj.Id == id);
        }


        public async Task<T> CreateAsync(T entity)
        {
            EntityEntry<T> obj = await _appDbContext.Set<T>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return obj.Entity;
        }


        public async Task<T?> UpdateAsync(T entity)
        {
            T? oldObj = await _appDbContext.Set<T>().FirstOrDefaultAsync(obj => obj.Id == entity.Id);

            if (oldObj == null)
                return null;

            _appDbContext.Entry(oldObj).CurrentValues.SetValues(entity);
            await _appDbContext.SaveChangesAsync();

            return oldObj;
        }


        public async Task<T?> DeleteAsync(Guid id)
        {
            T? obj = await _appDbContext.Set<T>().FirstOrDefaultAsync(obj => obj.Id == id);

            if (obj == null)
                return null;

            _appDbContext.Remove(obj);
            await _appDbContext.SaveChangesAsync();
            return obj;
        }
    }
}
