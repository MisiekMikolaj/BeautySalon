using BeautySalon.DataAcces.Data.Entities;
using BeautySalon.DataAcces.Data.Entities.Stuff;
using BeautySalon.DataAcces.Data.Entities.Users;

namespace BeautySalon.DataAcces.Data.Repositories
{
    public class SqlRepository<T> : IRepository<T>
        where T : class, IEntity, new()
    {
        private readonly BeautySalonDbContext _beautySalonDbContext;

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemove;

        public SqlRepository(BeautySalonDbContext beautySalonDbContext)
        {
            _beautySalonDbContext = beautySalonDbContext;
        }
        public void Add(T item)
        {
            _beautySalonDbContext.Add(item);
            ItemAdded?.Invoke(this, item);
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return _beautySalonDbContext.Employees.ToList() as List<T>;
            }
            else if (typeof(T) == typeof(Boss))
            {
                return _beautySalonDbContext.Bosses.ToList() as List<T>;
            }
            else if (typeof(T) == typeof(Client))
            {
                return _beautySalonDbContext.Clients.ToList() as List<T>;
            }
            else if (typeof(T) == typeof(WorkSchedule))
            {
                return _beautySalonDbContext.WorkSchedules.ToList() as List<T>;
            }
            else if (typeof(T) == typeof(Day))
            {
                return _beautySalonDbContext.Days.ToList() as List<T>;
            }
            else if (typeof(T) == typeof(Houer))
            {
                return _beautySalonDbContext.Houers.ToList() as List<T>;
            }
            else if (typeof(T) == typeof(Service))
            {
                return _beautySalonDbContext.Services.ToList() as List<T>;
            }
            else
            {
                return new List<T>();
            }
        }

        public T? GetById(int id)
        {
            var items = GetAll();
            return items.Single(i => i.Id == id);
        }

        public void Remove(T item)
        {
            _beautySalonDbContext.Remove(item);
            ItemRemove?.Invoke(this, item);
        }

        public void Save()
        {
            _beautySalonDbContext.SaveChanges(); 
        }
    }
}
