using BeautySalon.Entities;

namespace BeautySalon.Repositories
{
    public interface IReadRepository<out T>
        where T : class, IEntity
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
    }
}
