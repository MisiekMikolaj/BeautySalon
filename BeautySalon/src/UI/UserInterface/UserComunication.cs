using BeautySalon.DataAcces.Data;
using BeautySalon.DataAcces.Data.Entities;
using BeautySalon.DataAcces.Data.Entities.Stuff;
using BeautySalon.DataAcces.Data.Entities.Users;
using BeautySalon.DataAcces.Data.Repositories;
using BeautySalon.DataAcces.Data.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;

namespace BeautySalon.UI.UserInterface
{
    public class UserComunication : IUserComunication
    {
        public event EventHandler? SignToService;
        public string mainMenu => (
        " ------------------------------------------------- \n" +
        " | Select:                                       | \n" +
        " | 0)  if you want yo Log in                     | \n" +
        " | 1)  if you want add employee                  | \n" +
        " | 2)  if you want remove employee               | \n" +
        " | 3)  if you want display all employee          | \n" +
        " | 4)  if you want add boss                      | \n" +
        " | 5)  if you want clear console                 | \n" +
        " | 6)  if you want add work schedule to employee | \n" +
        " | 7)  if you want display work Schedules        | \n" +
        " | 8)  if you want add client                    | \n" +
        " | 9)  if you want sign up for a service         | \n" +
        " | 10) if you want display service list          | \n" +
        " | 11) if you want exit                          | \n" +
        " | 12) if you want Log out                       | \n" +
        " -------------------------------------------------");

        public string howManyEmployeeToAddMenu => ("Select: 1) if you want add 1 employee 2) if you want add many employees");

        public void AddEmployeeToDb(IRepository<Employee> employeeRepository)
        {
            Console.WriteLine("First name of employee: ");
            var firstName = Console.ReadLine();
            Console.WriteLine("Pasword of employee: ");
            var pasword = Console.ReadLine();
            employeeRepository.Add(new Employee { FirstName = firstName, Password = pasword });
            employeeRepository.Save();

        }
        public void AddEmployeesToDb(IRepository<Employee> employeeRepository)
        {
            List<Employee> listEmployees = new();

            var chose = "Add";
            while (chose == "Add")
            {
                Console.WriteLine("\nFirst name of employee: ");
                var firstName = Console.ReadLine();
                Console.WriteLine("Pasword of employee: ");
                var pasword = Console.ReadLine();
                listEmployees.Add(new Employee { FirstName = firstName, Password = pasword });

                Console.WriteLine("Do you want add next employee press + if don't press somethink else button ");
                chose = Console.ReadKey().Key.ToString();
            }
            employeeRepository.AddBatch(listEmployees);
            listEmployees.Clear();
        }

        public int TryToCheckIsItemIdExist<T>(IRepository<T> itemRepository) where T : EntityBase
        {
            try
            {
                var item = int.Parse(Console.ReadLine());
                itemRepository.GetById(item);
                return item;
            }
            catch
            {
                Console.WriteLine("" +
                    "\n!!!***********************!!!" +
                    "\n  Invalid or not existed Id" +
                    "\n!!!***********************!!!\n");
                return 0;
            }
        }

        public void RemoveEmployeeFromDb(IRepository<Employee> employeeRepository)
        {
            Console.WriteLine("Id employee which you want to remove");
            var employeeId = TryToCheckIsItemIdExist(employeeRepository);

            if (employeeId != 0)
            {
                employeeRepository.Remove(employeeRepository.GetById(employeeId));
                employeeRepository.Save();
            }
        }

        public void AddBossToDb(IRepository<Boss> bossRepository)
        {
            Console.WriteLine("First name of boss: ");
            var firstName = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();
            bossRepository.Add(new Boss { FirstName = firstName, Password = password });
            bossRepository.Save();
        }

        public void AddClientToDb(IRepository<Client> clientRepository)
        {
            Console.WriteLine("Email: ");
            var email = Console.ReadLine();
            Console.WriteLine("Pasword: ");
            var pasword = Console.ReadLine();
            clientRepository.Add(new Client { Email = email, Password = pasword });
            clientRepository.Save();
        }

        public void AddWorkScheduleToDb(IRepository<WorkSchedule> workScheduleRepository, IRepository<Employee> employeeRepository, int employeeId)
        {
            Console.Write("\nDate of this week monday(dd.MM.yyyy): ");

            try
            {
                var mondayDate = DateTime.Parse(Console.ReadLine());
                workScheduleRepository.Add(new WorkSchedule { Employee = employeeRepository.GetById(employeeId), EmployeeId = employeeId, Date = $"{mondayDate.ToString("dd/MM/yyyy")}-{mondayDate.AddDays(7).ToString("dd/MM/yyyy")}", Days = AddDaysOfWorkScheduleToList(mondayDate) });
                workScheduleRepository.Save();
            }
            catch
            {
                Console.WriteLine(
                    "\n!!!***********!!!" +
                    "\n  Invalid Value  " +
                    "\n!!!***********!!!\n");
            }
        }

        public List<Houer> AddHouersOfWorkScheduleToList(bool isToday)
        {
            List<string> houersWhenSalonIsOpen = new() { "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00" };
            List<Houer> houersInWork = new();

            if (isToday)
            {
                foreach (var houer in houersWhenSalonIsOpen)
                {
                    Console.Write($"\nAre you working at this houer {houer}? (true/false) ");
                    var isFree = bool.Parse(Console.ReadLine());
                    houersInWork.Add(new Houer { Time = houer, IsFree = isFree });
                }
            }
            return houersInWork;
        }

        public List<Day> AddDaysOfWorkScheduleToList(DateTime mondayDate)
        {
            List<string> daysWhenSalonIsOpen = new() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            List<Day> DaysInWork = new();

            var nextDay = 0;

            foreach (var day in daysWhenSalonIsOpen)
            {
                Console.Write($"\nAre you in job this day ({day} {mondayDate.AddDays(nextDay).ToString("dd/MM")})? (true/false) ");
                var isToday = bool.Parse(Console.ReadLine());
                DaysInWork.Add(new Day { IsToday = isToday, Name = day, Date = mondayDate.AddDays(nextDay).ToString("dd/MM"), Houers = AddHouersOfWorkScheduleToList(isToday) });
                nextDay += 1;
            }
            return DaysInWork;
        }

        public void DisplayAllWorkSchedules(IEnumerable<Employee> context)
        {
            foreach (Employee employee in context)
            {
                var employeeString = employee.ToString();
                foreach (WorkSchedule workSchedule in employee.WorkSchedules)
                {
                    var workScheduleString = workSchedule.ToString();
                    foreach (Day day in workSchedule.Days)
                    {
                        if (!String.IsNullOrEmpty(employeeString))
                        {
                            Console.WriteLine(employeeString);
                            employeeString = "";
                        }
                        if (!String.IsNullOrEmpty(workScheduleString))
                        {
                            Console.WriteLine(workScheduleString);
                            workScheduleString = "";
                        }
                        Console.WriteLine(day);

                        foreach (Houer houer in day.Houers)
                        {
                            Console.WriteLine(houer);
                        }
                    }
                }
                Console.WriteLine("\n");
            }
        }
        public void DisplayWorkSchedulesWhereHouersExist(IEnumerable<Employee> context)
        {
            foreach (Employee employee in context)
            {
                var employeeString = employee.ToString();
                foreach (WorkSchedule workSchedule in employee.WorkSchedules)
                {
                    var workScheduleString = workSchedule.ToString();
                    foreach (Day day in workSchedule.Days)
                    {
                        var dayString = day.ToString();
                        foreach (Houer houer in day.Houers)
                        {
                            if (!String.IsNullOrEmpty(employeeString))
                            {
                                Console.WriteLine(employeeString);
                                employeeString = "";
                            }
                            if (!String.IsNullOrEmpty(workScheduleString))
                            {
                                Console.WriteLine(workScheduleString);
                                workScheduleString = "";
                            }
                            if (!String.IsNullOrEmpty(dayString))
                            {
                                Console.WriteLine(dayString);
                                dayString = "";
                            }
                            Console.WriteLine(houer);
                        }
                    }
                }
                Console.WriteLine("\n");
            }
        }

        public void SignUpForService(BeautySalonDbContext beautySalonDbContext, int clientId)
        {
            Console.WriteLine("Service Id: ");
            var serviceId = int.Parse(Console.ReadLine());

            var client = beautySalonDbContext.Clients.Single(c => c.Id == clientId).Email.ToString();
            var service = beautySalonDbContext.Services.Single(s => s.Id == serviceId).Name.ToString();

            var context = makeContextToSignUpForService(beautySalonDbContext);
            foreach (Employee employee in context)
            {
                foreach (WorkSchedule workShedule in employee.WorkSchedules)
                {
                    foreach (Day day in workShedule.Days)
                    {
                        foreach (Houer houer in day.Houers)
                        {
                            if(!day.IsToday || !houer.IsFree)
                            {
                                Console.WriteLine(
                                    "\n" +
                                    "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n" +
                                    "  This employee is not free this time  \n" +
                                    "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
                            }
                            if (day.IsToday && houer.IsFree)
                            {
                                houer.IsFree = false;
                                houer.Client = client;
                                houer.Service = service;
                            }
                        }
                    }
                }
            }
            beautySalonDbContext.SaveChanges();  
        }

        public List<Employee> makeContextToSignUpForService(BeautySalonDbContext beautySalonDbContext)
        {
            beautySalonDbContext.ChangeTracker.Clear();

            Console.WriteLine("Employee Id: ");
            var employeeId = int.Parse(Console.ReadLine());
            Console.WriteLine("Date(dd.MM): ");
            var visitDate = Console.ReadLine();
            Console.WriteLine("Houer (HH:mm): ");
            var visitHouer = Console.ReadLine();

            var context = beautySalonDbContext.Employees.Where(e => e.Id == employeeId)
                .Include(e => e.WorkSchedules)
                .ThenInclude(w => w.Days.Where(d => d.Date == visitDate))
                .ThenInclude(d => d.Houers.Where(h => h.Time == visitHouer))
                .ToList();

            return context;
        }

        public bool CheckAccess(Type userType, Type accesType)
        {
            if (userType == accesType)
            {
                return true;
            }
            else
            {
                Console.WriteLine(
                    $"\n******************************************" +
                    $"\n   You must be a {accesType.Name} to do it" +
                    $"\n******************************************\n");
                return false;
            }
        }

        public bool CheckAccess(Type userType, Type accesType, Type alternativeAccesType)
        {
            if ((userType == accesType) || (userType == alternativeAccesType))
            {
                return true;
            }
            else
            {
                Console.WriteLine(
                    $"\n******************************************" +
                    $"\n   You must be a {accesType.Name} to do it" +
                    $"\n******************************************\n");
                return false;
            }
        }

        public ActualUser ReturnUser<T>(IRepository<T> repository, ActualUser actualUser) where T : UserEntityBase
        {
            Console.WriteLine("Your Id: ");
            var userId = TryToCheckIsItemIdExist(repository);
            if (userId != 0)
            {
                Console.WriteLine("Your Password: ");
                var userPassword = Console.ReadLine();

                if (repository.GetById(userId).Password == userPassword)
                {
                    var userType = repository.GetById(userId).GetType();
                    return new ActualUser()
                    {
                        id = userId,
                        type = userType,
                    };
                }
                else
                {
                    Console.WriteLine(
                        "********************" +
                        "*  Wrong Password  *" +
                        "********************");
                    return actualUser;
                }
            }
            else
            {
                return actualUser;
            }
        }
    }
}
