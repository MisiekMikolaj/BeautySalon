using BeautySalon.DataAcces.Data.Entities;
using BeautySalon.DataAcces.Data.Entities.Users;
using BeautySalon.DataAcces.Data.Repositories;
using System.Text.Json;

namespace BeautySalon.ApplicationServices.Components
{
    public class TxtReader : ITxtReader
    {
        public void ReadEmployeeFromFileToDb(IRepository<Employee> repository, string path)
        {
            using (var reader = File.OpenText(path))
            {
                var line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var item = JsonSerializer.Deserialize<Employee>(line);
                    repository.Add(new Employee()
                    {
                        FirstName = item.FirstName,
                        Password = item.Password
                    });
                    line = reader.ReadLine();
                }
                repository.Save();
            }
        }

        public void ReadItemFromFileToDb<T>(IRepository<T> repository, string path) where T : class, IEntity
        {
            using (var reader = File.OpenText(path))
            {
                var line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var item = JsonSerializer.Deserialize<T>(line);
                    repository.Add(item);
                    line = reader.ReadLine();
                }
                repository.Save();
            }
        }
    }
}