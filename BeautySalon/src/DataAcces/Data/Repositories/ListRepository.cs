using BeautySalon.DataAcces.Data.Entities;
using BeautySalon.DataAcces.Data.Entities.Users;

namespace BeautySalon.DataAcces.Data.Repositories
{
    public class ListRepository<T> : IRepository<T>
        where T : class, IEntity, new()
    {
        public readonly List<T> _items = new();

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemove;

        public void Add(T item)
        {
            //item.Id = _items.Count + 1;
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public void Save()
        {
            //Not required in ListRepository
        }

        public T? GetById(int id) => _items.Single(item => item.Id == id);

        public IEnumerable<T> GetAll() => _items.ToList();
    }
}
