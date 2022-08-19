using BeautySalon.DataAcces.Data.Entities;

namespace BeautySalon.DataAcces.Data.Repositories
{
    public interface IReadRepository<out T>
        where T : class, IEntity
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
    }
}
