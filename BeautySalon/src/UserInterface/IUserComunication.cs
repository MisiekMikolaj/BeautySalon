using BeautySalon.Data;
using BeautySalon.Entities.Stuff;
using BeautySalon.Entities.Users;
using BeautySalon.Repositories;
using Microsoft.EntityFrameworkCore.Query;

namespace BeautySalon.UserInterface
{
    public interface IUserComunication
    {
        public string mainMenu { get; }
        public string howManyEmployeeToAddMenu { get; }

        event EventHandler? SignToService;

        public void AddEmployeeToDb(IRepository<Employee> employeeRepository);
        public void AddEmployeesToDb(IRepository<Employee> employeeRepository, List<Employee> listEmployees);
        public void RemoveEmployeeFromDb(IRepository<Employee> employeeRepository);
        public void AddBossToDb(IRepository<Boss> bossRepository);
        public void AddClientToDb(IRepository<Client> clientRepository);
        public void AddWorkScheduleToDb(IRepository<WorkSchedule> workScheduleRepository);
        public void DisplayAllWorkSchedules(IList<Employee> context);
        public void SignUpForService(BeautySalonDbContext beautySalonDbContext, IRepository<Client> clientRepository, int clientId, int employeeId, string date, string houer, IRepository<Service> service, int serviceId);
        public void SignForServiceUsedConsole(BeautySalonDbContext beautySalonDbContext,IRepository<Client> clientRepository, IRepository<Service> service);
    }
}
