using BeautySalon.ApplicationServices.Components;
using BeautySalon.DataAcces.Data;
using BeautySalon.DataAcces.Data.Entities.Stuff;
using BeautySalon.DataAcces.Data.Entities.Users;
using BeautySalon.DataAcces.Data.Repositories;
using BeautySalon.UI.UserInterface;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.UI
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
        private readonly ITxtReader _txtReader;
        private readonly ITxtWritter _txtWritter;
        public App(
               IRepository<Employee> employeerepository,
               IRepository<Boss> bossrepository,
               IRepository<Client> clientRepository,
               BeautySalonDbContext beautySalonDbContext,
               IUserComunication userComunication,
               IRepository<Service> serviceRepository,
               IRepository<WorkSchedule> workScheduleRepository,
               IRepository<Day> dayRepository,
               IRepository<Houer> houerRepository,
               ITxtReader txtReader,
               ITxtWritter txtWritter)
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
            _txtReader = txtReader;
            _txtWritter = txtWritter;
        }

        public void Run()
        {
            if (_serviceRepository.GetAll().Count() == 0)
            {
                _serviceRepository.Add(new Service { Name = "Hair cut", Price = 15, Time = 60 });
                _serviceRepository.Add(new Service { Name = "Beard trim", Price = 20, Time = 60 });
                _serviceRepository.Save();
            }
            if (_clientRepository.GetAll().Count() == 0)
            {
                _clientRepository.Add(new Client { Email = "EMAIL@COM", Password = "lol" });
                _clientRepository.Save();
            }
            if (_employeeRepository.GetAll().Count() == 0)
            {
                _txtReader.ReadEmployeeFromFileToDb(_employeeRepository, @"Entities\Employee.txt");
            }
            if (_bossRepository.GetAll().Count() == 0)
            {
                _txtReader.ReadItemFromFileToDb(_bossRepository, @"Entities\Boss.txt");
            }
            if (_workScheduleRepository.GetAll().Count() == 0)
            {
                _employeeRepository.GetById(1).WorkSchedules.Add(new WorkSchedule() { Date = "12-15", Days = new List<Day> { new Day { IsToday = true, Name = "poniedziałek", Date = "12.01", Houers = new List<Houer> { new Houer { Time = "08:00", IsFree = true }, new Houer { Time = "09:00", IsFree = false } } } } });
                _employeeRepository.Save();
            }

            _employeeRepository.ItemAdded += EmployeeRepositoryFromItemAdded;
            _bossRepository.ItemAdded += EmployeeRepositoryFromItemAdded;
            _employeeRepository.ItemRemove += EmployeeRepositoryFromItemRemove;
            _bossRepository.ItemRemove += EmployeeRepositoryFromItemRemove;
            _clientRepository.ItemAdded += ClientRepositoryFromItemAdded;
            _workScheduleRepository.ItemAdded += WorkScheduleRepositoryFromItemAdded;

            ActualUser user = new ActualUser { id = 0, type = null};
            while (true)
            {
                Console.WriteLine(_userComunication.mainMenu);
                Console.Write("\nChose : ");

                var choseMainMenu = Console.ReadLine();
                switch (choseMainMenu)
                {
                    case "0":
                        Console.WriteLine("Select: " +
                            "\n1) Employee" +
                            "\n2) Client");
                        var choseUserType = Console.ReadLine();
                        switch(choseUserType)
                        {
                            case "1":
                                user = _userComunication.ReturnUser(_employeeRepository, user);
                                break;
                            case "2":
                                user = _userComunication.ReturnUser(_clientRepository, user);
                                break;
                        }
                        break;
                    case "1":
                        if (_userComunication.CheckAccess(user.type, typeof(Boss)))
                        {
                            Console.WriteLine(_userComunication.howManyEmployeeToAddMenu);
                            var choseHowManyEmployees = Console.ReadLine();
                            switch (choseHowManyEmployees)
                            {
                                case "1":
                                    _userComunication.AddEmployeeToDb(_employeeRepository);
                                    break;
                                case "2":
                                    _userComunication.AddEmployeesToDb(_employeeRepository);
                                    break;
                            }
                        }
                        break;
                    case "2":
                        if (_userComunication.CheckAccess(user.type, typeof(Boss)))
                        {
                            _userComunication.RemoveEmployeeFromDb(_employeeRepository);
                        }
                        break;
                    case "3":
                        Console.WriteLine("Select: " +
                            "\n1) if you want display all employees ordered by id" +
                            "\n2) if you want display all employees ordered by name" +
                            "\n3) if you want display employee by name");
                        var choseEmployeeSelection = Console.ReadLine();
                        switch (choseEmployeeSelection)
                        {
                            case "1":
                                foreach (var employee in _employeeRepository.GetAll().OrderBy(e => e.Id))
                                {
                                    Console.WriteLine(employee);
                                }
                                break;
                            case "2":
                                foreach (var employee in _employeeRepository.GetAll().OrderBy(e => e.FirstName))
                                {
                                    Console.WriteLine(employee);
                                }
                                break;
                            case "3":
                                Console.WriteLine("Employee with what prefix you looking for?");
                                var prefix = Console.ReadLine();
                                foreach (var employee in _employeeRepository.GetAll().Where(e => e.FirstName.StartsWith(prefix)))
                                {
                                    Console.WriteLine(employee);
                                }
                                break;
                        }
                        break;
                    case "4":
                        if (_userComunication.CheckAccess(user.type, typeof(Boss)))
                        {
                            _userComunication.AddBossToDb(_bossRepository);
                        }
                        break;
                    case "5":
                        Console.Clear();
                        break;
                    case "6":
                        if (_userComunication.CheckAccess(user.type, typeof(Employee), typeof(Boss)))
                        {
                            _userComunication.AddWorkScheduleToDb(_workScheduleRepository, _employeeRepository, user.id);
                        }
                        break;
                    case "7":
                        Console.WriteLine("Select: " +
                            "\n1) to display all Work Schedules " +
                            "\n2) to display Work Schedules of one employee" +
                            "\n3) to display Work Schedules where employee is free");
                        Console.Write("\nChose: ");
                        var choseSelectionWorkSchedule = Console.ReadLine();
                        switch (choseSelectionWorkSchedule)
                        {
                            case "1":
                                var context = _beautySalonDbContext.Employees.Where(e => e.WorkSchedules.Count() > 0)
                                    .Include(e => e.WorkSchedules).ThenInclude(w => w.Days)
                                    .ThenInclude(d => d.Houers)
                                    .AsNoTracking().ToList();
                                _userComunication.DisplayAllWorkSchedules(context);
                                break;
                            case "2":
                                Console.Write("Employee Id: ");
                                var employeeId = _userComunication.TryToCheckIsItemIdExist(_employeeRepository);
                                if (employeeId != 0)
                                {
                                    context = _beautySalonDbContext.Employees.Where(e => e.Id == employeeId)
                                        .Include(e => e.WorkSchedules)
                                        .ThenInclude(w => w.Days)
                                        .ThenInclude(d => d.Houers)
                                        .AsNoTracking().ToList();

                                    _userComunication.DisplayAllWorkSchedules(context);
                                }

                                break;
                            case "3":
                                context = _beautySalonDbContext.Employees.Where(e => e.WorkSchedules.Count() > 0)
                                     .Include(e => e.WorkSchedules.Where(w => w.Days.Count() > 0))
                                     .ThenInclude(w => w.Days.Where(d => d.IsToday && d.Houers.Count() > 0))
                                     .ThenInclude(d => d.Houers.Where(h => h.IsFree))
                                     .AsNoTracking().ToList();
                                _userComunication.DisplayWorkSchedulesWhereHouersExist(context);
                                break;
                        }
                        break;
                    case "8":
                            _userComunication.AddClientToDb(_clientRepository);
                        break;
                    case "9":
                        if (_userComunication.CheckAccess(user.type, typeof(Client)))
                        {
                            try
                            {
                                _userComunication.SignUpForService(_beautySalonDbContext, user.id);
                            }
                            catch
                            {
                                Console.WriteLine(
                                    "\n!!!***********!!!" +
                                    "\n  Invalid Value  " +
                                    "\n!!!***********!!!\n");
                            }
                        }
                        break;
                    case "10":
                        foreach (var service in _serviceRepository.GetAll())
                        {
                            Console.WriteLine(service);
                        }
                        break;
                    case "11":
                        Environment.Exit(0);
                        break;
                    case "12":
                        user.type = null;
                        user.id = 0;
                        break;
                }
            }
        }

        void EmployeeRepositoryFromItemAdded(object? sender, Employee e)
        {
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine($"{e.GetType().Name} added {e.FirstName} from {sender?.GetType().Name}\n");
            _txtWritter.AuditSaveInFile(e, "added", e.FirstName);
        }
        void EmployeeRepositoryFromItemRemove(object? sender, Employee e)
        {
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine($"\n{e.GetType().Name} remove {e.FirstName} from {sender?.GetType().Name}\n");
            _txtWritter.AuditSaveInFile(e, "remove", e.FirstName);
        }
        void ClientRepositoryFromItemAdded(object? sender, Client c)
        {
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine($"\n{c.GetType().Name} added {c.Email} from {sender?.GetType().Name}\n");
            _txtWritter.AuditSaveInFile(c, "added", c.Email);
        }
        void WorkScheduleRepositoryFromItemAdded(object? sender, WorkSchedule w)
        {
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine($"\n{w.GetType().Name} added to Employye {w.Employee.FirstName} with Id: {w.EmployeeId} from {sender?.GetType().Name}\n");
            _txtWritter.AuditSaveInFile(w, $"added to Employee {w.Employee.FirstName} with Id: ", w.EmployeeId);
        }
    }
}
