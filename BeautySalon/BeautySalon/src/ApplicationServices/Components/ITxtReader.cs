using BeautySalon.DataAcces.Data.Entities;
using BeautySalon.DataAcces.Data.Entities.Users;
using BeautySalon.DataAcces.Data.Repositories;

namespace BeautySalon.ApplicationServices.Components
{
    public interface ITxtReader
    {
        public void ReadItemFromFileToDb<T>(IRepository<T> repository, string path) where T : class, IEntity;
        public void ReadEmployeeFromFileToDb(IRepository<Employee> repository, string path);
    }
}
