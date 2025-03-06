using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly AppDbContext _appDbContext;

        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _appDbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Set<T>().FirstOrDefaultAsync(obj => obj.Id == id);
        }


        public async Task<T> CreateAsync(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }


        public async Task<T> UpdateAsync(T entity)
        {
            T oldObj = await _appDbContext.Set<T>().FirstOrDefaultAsync(obj => obj.Id == entity.Id);

            if (oldObj == null)
                return null;

            _appDbContext.Entry(oldObj).CurrentValues.SetValues(entity);
            await _appDbContext.SaveChangesAsync();

            return oldObj;
        }


        public async Task<T> DeleteAsync(Guid id)
        {
            T obj = await _appDbContext.Set<T>().FirstOrDefaultAsync(obj => obj.Id == id);
            _appDbContext.Remove(obj);

            await _appDbContext.SaveChangesAsync();
            return obj;
        }
    }
}
