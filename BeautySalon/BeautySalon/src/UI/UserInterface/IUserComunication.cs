using BeautySalon.DataAcces.Data;
using BeautySalon.DataAcces.Data.Entities;
using BeautySalon.DataAcces.Data.Entities.Stuff;
using BeautySalon.DataAcces.Data.Entities.Users;
using BeautySalon.DataAcces.Data.Repositories;
using Microsoft.EntityFrameworkCore.Query;

namespace BeautySalon.UI.UserInterface
{
    public interface IUserComunication
    {
        public string mainMenu { get; }
        public string howManyEmployeeToAddMenu { get; }

        event EventHandler? SignToService;

        public void AddEmployeeToDb(IRepository<Employee> employeeRepository);
        public void AddEmployeesToDb(IRepository<Employee> employeeRepository);
        public void RemoveEmployeeFromDb(IRepository<Employee> employeeRepository);
        public void AddBossToDb(IRepository<Boss> bossRepository);
        public void AddClientToDb(IRepository<Client> clientRepository);
        public void AddWorkScheduleToDb(IRepository<WorkSchedule> workScheduleRepository, IRepository<Employee> employeeRepository, int employeeId);
        public void DisplayAllWorkSchedules(IEnumerable<Employee> context);
        public void DisplayWorkSchedulesWhereHouersExist(IEnumerable<Employee> context);
        public void SignUpForService(BeautySalonDbContext beautySalonDbContext, int clientId);
        public int TryToCheckIsItemIdExist<T>(IRepository<T> itemRepository) where T : EntityBase;
        public bool CheckAccess(Type userType, Type AccesType);
        public bool CheckAccess(Type userType, Type accesType, Type alternativeAccesType);
        public ActualUser ReturnUser<T>(IRepository<T> repository, ActualUser actualUser) where T : UserEntityBase;
    }
}
