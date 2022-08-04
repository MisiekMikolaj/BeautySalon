using BeautySalon.Data;
using BeautySalon.Entities.Stuff;
using BeautySalon.Entities.Users;
using BeautySalon.Repositories;
using BeautySalon.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BeautySalon.UserInterface
{
    public class UserComunication : IUserComunication
    {
        public event EventHandler? SignToService;
        public string mainMenu => (
        " ------------------------------------------------- \n" +
        " | Select:                                       | \n" +
        " | 1)  if you want add employee                  | \n" +
        " | 2)  if you want remove employee               | \n" +
        " | 3)  if you want display all employee          | \n" +
        " | 4)  if you want add boss                      | \n" +
        " | 5)  if you want clear console                 | \n" +
        " | 6)  if you want add work schedule to employee | \n" +
        " | 7)  if you want display work Schedules        | \n" +
        " | 8)  if you want add client                    | \n" +
        " | 9)  if you want sign up for a service         | \n" +
        " | 10) if you want exit                          | \n" +
        " -------------------------------------------------");

        public string howManyEmployeeToAddMenu => ("Select: 1) if you want add 1 employee 2) if you want add many employees");
        
        public void AddEmployeeToDb(IRepository<Employee> employeeRepository)
        {
            Console.WriteLine("First name of employee: ");
            var firstName = Console.ReadLine();
            employeeRepository.Add(new Employee { FirstName = firstName });
            employeeRepository.Save();
        }
        public void AddEmployeesToDb(IRepository<Employee> employeeRepository, List<Employee> listEmployees)
        {
            var chose = "Add";
            while(chose == "Add")
            {
                Console.WriteLine("\nFirst name of employee: ");
                var firstName = Console.ReadLine();
                listEmployees.Add(new Employee { FirstName = firstName });

                Console.WriteLine("Do you want add next employee press + if don't press somethink else button ");
                chose = Console.ReadKey().Key.ToString();
            }
            employeeRepository.AddBatch(listEmployees);
        }

        public void RemoveEmployeeFromDb(IRepository<Employee> employeeRepository)
        {
            Console.WriteLine("Id employee which you want to remove");
            var id = int.Parse(Console.ReadLine());
            employeeRepository.Remove(employeeRepository.GetById(id));
            employeeRepository.Save();
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
            clientRepository.Add(new Client { Email = email });
            clientRepository.Save();
        }

        public void AddWorkScheduleToDb(IRepository<WorkSchedule> workScheduleRepository)
        {
            List<string> days = new() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            List<string> houers = new() { "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00" };

            Console.Write("\nEmployee Id: ");
            var id = int.Parse(Console.ReadLine());
            Console.Write("\nDate of this week monday(dd.MM.yyyy): ");
            var mondayDate = DateTime.Parse(Console.ReadLine());
            var nextDay = 0;

            List<Day> Days = new();
            List <Houer> Houers = new();   

            foreach (var day in days)
            {
                Console.Write($"\nAre you in job this day ({day} {mondayDate.AddDays(nextDay).ToString("dd/MM")})? (true/false) ");
                var isToday = bool.Parse(Console.ReadLine());
                if (isToday == true)
                {
                    foreach (var houer in houers)
                    {
                        Console.Write($"\nAre you working at this houer {houer}? (true/false) ");
                        var free = bool.Parse(Console.ReadLine());
                        Houers.Add(new Houer { Time = houer, Free = free });

                    }
                    Days.Add(new Day { IsToday = isToday, Name = day, Date = mondayDate.AddDays(nextDay).ToString("dd/MM"), Houers = new List<Houer>(Houers) });
                    Houers.Clear();
                }
                nextDay += 1;
            }
            workScheduleRepository.Add(new WorkSchedule { EmployeeId = id, Date = $"{mondayDate.ToString("dd/MM/yyyy")}-{mondayDate.AddDays(7).ToString("dd/MM/yyyy")}", Days = Days });
            workScheduleRepository.Save();
           /* employeeRepository.GetById(id).WorkSchedules.Add(new WorkSchedule()
            {
                Date = $"{mondayDate.ToString("dd/MM/yyyy")}-{mondayDate.AddDays(7).ToString("dd/MM/yyyy")}",
                Days = Days
            });
            employeeRepository.Save();*/
        }

        public void DisplayAllWorkSchedules(IList<Employee> context)
        {
            foreach (Employee e in context)
            {
                Console.WriteLine("\n" + e.ToString());
                foreach (WorkSchedule w in e.WorkSchedules)
                {
                    Console.WriteLine(w.ToString());
                    foreach (Day d in w.Days)
                    {
                        Console.WriteLine(d.ToString());
                        foreach (Houer h in d.Houers)
                        {
                            Console.WriteLine(h.ToString());
                        }
                    }
                }
            }
        }

        public void SignUpForService(BeautySalonDbContext beautySalonDbContext,IRepository<Client> clientRepository, int clientId, int employeeId, string date, string houer, IRepository<Service> serviceRepository, int serviceId)
        {
            var client = clientRepository.GetById(clientId).Email.ToString();
            var service = serviceRepository.GetById(serviceId).Name.ToString();
            var context = beautySalonDbContext.Employees.Include(e => e.WorkSchedules).ThenInclude(w => w.Days.Where(d => d.Date == date)).ThenInclude(d => d.Houers.Where(h => h.Time == houer)).Where(e => e.Id == employeeId);

            foreach (Employee e in context)
            {
                foreach (WorkSchedule w in e.WorkSchedules)
                {
                    foreach (Day d in w.Days)
                    {
                        foreach (Houer h in d.Houers)
                        {
                            if(d.IsToday == true && h.Free == true )
                            {
                                h.Free = false;
                                h.Client = client;
                                h.Service = service; 
                            }
                        }
                    }
                }
            }
            beautySalonDbContext.SaveChanges();
        }

        public void SignForServiceUsedConsole(BeautySalonDbContext beautySalonDbContext, IRepository<Client> clientRepository, IRepository<Service> service)
        {
            Console.WriteLine("Client Id: ");
            var clientId = int.Parse(Console.ReadLine());
            Console.WriteLine("Employee Id: ");
            var employeeId = int.Parse(Console.ReadLine());
            Console.WriteLine("Date(dd.MM): ");
            var date = Console.ReadLine();
            Console.WriteLine("Houer (HH:mm): ");
            var houer = Console.ReadLine();
            Console.WriteLine("Service Id: ");
            var servicetId = int.Parse(Console.ReadLine());

            SignUpForService(beautySalonDbContext, clientRepository, clientId, employeeId, date, houer, service, servicetId);
        }
    }   
}
