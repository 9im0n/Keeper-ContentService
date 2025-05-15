using Keeper_ContentService.Models.Db;

namespace Keeper_ContentService.Repositories.BaseRepository.Interfaces
{
    public interface IBaseRepository<T> :
    IReadableRepository<T>,
    IWritableRepository<T>,
    IDeletableRepository<T>
    where T : BaseModel
    { }
}
