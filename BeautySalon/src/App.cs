using BeautySalon.Data;
using BeautySalon.Entities;
using BeautySalon.Entities.Stuff;
using BeautySalon.Entities.Users;
using BeautySalon.Repositories;
using BeautySalon.Repositories.Extensions;
using BeautySalon.UserInterface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;



namespace BeautySalon
{
    public class App : IApp
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Boss> _bossRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly BeautySalonDbContext _beautySalonDbContext;
        private readonly IUserComunication _userComunication;
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<WorkSchedule> _workScheduleRepository;
        private readonly IRepository<Day> _dayRepository;
        private readonly IRepository<Houer> _houerRepository;
        public App(
               IRepository<Employee> employeerepository,
               IRepository<Boss> bossrepository,
               IRepository<Client> clientRepository,
               BeautySalonDbContext beautySalonDbContext,
               IUserComunication userComunication,
               IRepository<Service> serviceRepository,
               IRepository<WorkSchedule> workScheduleRepository,
               IRepository<Day> dayRepository,
               IRepository<Houer> houerRepository)
        {
            _employeeRepository = employeerepository;
            _bossRepository = bossrepository;
            _clientRepository = clientRepository;
            _beautySalonDbContext = beautySalonDbContext;
            _serviceRepository = serviceRepository;
            _beautySalonDbContext.Database.EnsureCreated();
            _userComunication = userComunication;
            _workScheduleRepository = workScheduleRepository;
            _dayRepository = dayRepository;
            _houerRepository = houerRepository;
        }

        public void Run()
        {
            _serviceRepository.Add(new Service { Name = "Hair cut", Price = 15, Time = 1 });
            _serviceRepository.Add(new Service { Name = "Beard trim", Price = 20, Time = 1 });
            _serviceRepository.Save();

            _clientRepository.Add(new Client { Email = "EMAIL@COM" });
            _clientRepository.Save();

            _employeeRepository.Add(new Employee { FirstName = "Mikołaj" });
            _employeeRepository.Save();

            _bossRepository.Add(new Boss { FirstName = "Natalia", Password = "lol" });
            _bossRepository.Save();

            _employeeRepository.GetById(1).cos();
            _employeeRepository.Save();

            _employeeRepository.ItemAdded += EmployeeRepositoryFromItemAdded;
            _bossRepository.ItemAdded += EmployeeRepositoryFromItemAdded;
            _employeeRepository.ItemRemove += EmployeeRepositoryFromItemRemove;
            _bossRepository.ItemRemove += EmployeeRepositoryFromItemRemove;
            _clientRepository.ItemAdded += ClientRepositoryFromItemAdded;
            _workScheduleRepository.ItemAdded += WorkScheduleRepositoryFromItemAdded;

            Console.WriteLine(_userComunication.mainMenu);
            Console.Write("\nChose : ");


            while (true)
            {
                    var choseMainMenu = Console.ReadLine();
                    switch (choseMainMenu)
                    {
                        case "1":
                            Console.WriteLine(_userComunication.howManyEmployeeToAddMenu);
                            var choseHowManyEmployees = Console.ReadLine();
                            switch (choseHowManyEmployees)
                            {
                                case "1":
                                    _userComunication.AddEmployeeToDb(_employeeRepository);
                                    break;
                                case "2":
                                    List<Employee> emplyeesList = new();
                                    _userComunication.AddEmployeesToDb(_employeeRepository, emplyeesList);
                                    emplyeesList.Clear();
                                    break;
                            }
                            break;
                        case "2":
                            _userComunication.RemoveEmployeeFromDb(_employeeRepository);
                            break;
                        case "3":
                            foreach (var employee in _employeeRepository.GetAll())
                            {
                                Console.WriteLine(employee);
                            }
                            break;
                        case "4":
                            _userComunication.AddBossToDb(_bossRepository);
                            break;
                        case "5":
                            Console.Clear();
                            Console.WriteLine(_userComunication.mainMenu);
                            break;
                        case "6":
                            _userComunication.AddWorkScheduleToDb(_workScheduleRepository);
                            break;
                        case "7":
                            Console.WriteLine("Select: \n1) to display all Work Schedule "+ 
                                                     "\n2) to display Work Schedule of one employee"+
                                                     "\n3) to display Work Schedule where employee is free");
                            var choseSelectionWorkSchedule = Console.ReadLine();                        
                            switch (choseSelectionWorkSchedule)
                            {
                                case "1":
                                    var context = _beautySalonDbContext.Employees.Include(e => e.WorkSchedules).ThenInclude(w => w.Days).ThenInclude(d => d.Houers).ToList();
                                    _userComunication.DisplayAllWorkSchedules(context);
                                break;
                                case "2":
                                    Console.WriteLine("Employee Id: ");
                                    var empId = int.Parse(Console.ReadLine());
                                    context = _beautySalonDbContext.Employees.Where(e => e.Id == empId).Include(e => e.WorkSchedules).ThenInclude(w => w.Days).ThenInclude(d => d.Houers).ToList();
                                    _userComunication.DisplayAllWorkSchedules(context);
                                break;
                                case "3":
                                    context = _beautySalonDbContext.Employees.Where(e => e.WorkSchedules.Count() > 0).Include(e => e.WorkSchedules.Where( w => w.Days.Count() >0)).ThenInclude(w => w.Days.Where(d => d.IsToday == true && d.Houers.Count() > 0)).ThenInclude(d => d.Houers.Where(h => h.Free == true)).ToList();
                                    _userComunication.DisplayAllWorkSchedules(context);
                                break;
                            }
                        break;
                        case "8":
                            _userComunication.AddClientToDb(_clientRepository);
                        break;
                        case "9":
                        _userComunication.SignForServiceUsedConsole(_beautySalonDbContext, _clientRepository, _serviceRepository);
                        break;
                        case "10":
                            Environment.Exit(0);
                        break;
                    }
                    Console.Write("\nChose: ");

            }

            /*var ss = new SaveToFile();

            ss.ReadFromFile(_bossRepository, @"Entities\Boss.txt");
            ss.ReadFromFile(_employeeRepository, @"Entities\Employee.txt");

            foreach(var i in _employeeRepository.GetAll())
            {
                Console.WriteLine(i);
            }*/

            /*using (var reader = File.OpenText(@"Entities\Boss.txt"))
            {
                var line = reader.ReadLine();
                while (!String.IsNullOrEmpty(line))
                {
                    var item = JsonSerializer.Deserialize<Boss>(line);
                    _beautySalonDbContext.Employees.Add(new Boss()
                    {
                        FirstName = item.FirstName,
                        //Id = item.Id,
                        Password = item.Password
                    }); 
                    line = reader.ReadLine();
                }
                _beautySalonDbContext.SaveChanges();
            }*/
            /*var emp = _beautySalonDbContext.Employees.ToList();
            _beautySalonDbContext.Remove(emp[1]);
            _beautySalonDbContext.SaveChanges();*/

            /*_beautySalonDbContext.Add(new Employee { FirstName = "Koles" });
            _beautySalonDbContext.SaveChanges();*/


        }

        static void EmployeeRepositoryFromItemAdded(object? sender, Employee e)
        {
            Console.WriteLine($"\n{e.GetType().Name} added {e.FirstName} from {sender?.GetType().Name}\n");
            SaveToFile.AuditSaveInFile(e, "added", e.FirstName);
        }
        static void EmployeeRepositoryFromItemRemove(object? sender, Employee e)
        {
            Console.WriteLine($"\n{e.GetType().Name} remove {e.FirstName} from {sender?.GetType().Name}\n");
            SaveToFile.AuditSaveInFile(e, "remove", e.FirstName);
        }
        static void ClientRepositoryFromItemAdded(object? sender, Client c)
        {
            Console.WriteLine($"\n{c.GetType().Name} added {c.Email} from {sender?.GetType().Name}\n");
            SaveToFile.AuditSaveInFile(c, "added", c.Email);
        }
        static void WorkScheduleRepositoryFromItemAdded(object? sender, WorkSchedule w)
        {
            Console.WriteLine($"\n{w.GetType().Name} added Work Schedule to Employye with Id: {w.EmployeeId} from {sender?.GetType().Name}\n");
            SaveToFile.AuditSaveInFile(w, "added to Employee with Id: ", w.EmployeeId);
        }
    }
}
