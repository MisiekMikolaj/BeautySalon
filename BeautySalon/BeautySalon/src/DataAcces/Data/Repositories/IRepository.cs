using BeautySalon.DataAcces.Data.Entities;
using BeautySalon.DataAcces.Data.Entities.Users;

namespace BeautySalon.DataAcces.Data.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : class, IEntity
    {
        event EventHandler<T>? ItemAdded;
        event EventHandler<T>? ItemRemove;
    }
}
