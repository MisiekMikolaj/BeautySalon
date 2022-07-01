using BeautySalon.Entities;

namespace BeautySalon.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> 
        where T : class, IEntity
    {   
    }
}
