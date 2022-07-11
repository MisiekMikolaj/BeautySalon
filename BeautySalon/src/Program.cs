using BeautySalon.Data;
using BeautySalon.Entities;
using BeautySalon.Repositories;
using BeautySalon.Repositories.Extensions;
using BeautySalon.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Runtime.CompilerServices;

var employeeRepository = new SqlRepository<Employee>(new BeautySalonDbContext());
var bossRepository = new SqlRepository<Boss>(new BeautySalonDbContext());
var ss = new SaveToFile();

ss.ReadFromFile(bossRepository, @"Entities\Boss.txt");
ss.ReadFromFile(employeeRepository, @"Entities\Employee.txt");

employeeRepository.ItemAdded += EmployeeRepositoryFromItemAdded;
bossRepository.ItemAdded += EmployeeRepositoryFromItemAdded;
employeeRepository.ItemRemove += EmployeeRepositoryFromItemRemove;
bossRepository.ItemRemove += EmployeeRepositoryFromItemRemove;

/*var services = new ServiceCollection();
services.AddDbContext<BeautySalonDbContext>(options => options
    .UseSqlServer("Data Source=DESKTOP-24JQ58J\\SQLEXPRESS;Initial Catalog=BeautySalonStorage;Integrated Security=True"));*/
var menu = (" --------------------------------------------\n | Select:                                  | " +
        "\n | 1) if you want add employee              | \n | 2) if you want remove employee           | \n " +
        "| 3) if you want display all employee      | \n | 4) if you want add boss                  | \n " +
        "| 5) if you want clear console             | \n | 6) if you want save changes              " +
        "| \n | 7) if you want exit                      " +
        "|\n --------------------------------------------");
Console.WriteLine("!!!!!!!!!! Boss Id: 1, Boss Pasword: lol !!!!!!!!!!!!!!!");
Console.WriteLine(menu);
Console.Write("\nChose : ");

bool startStop = true;
while(startStop)
{
    var chose = Console.ReadLine();
    switch (chose)
    {
        case "1":
            Console.Write("give name of employee who you want to add: ");
            var empName = Console.ReadLine();
            AddEmployeesFromConsole(employeeRepository, empName);
            break;
        case "2":
            Console.Write("give id of employee who you want to remowe: ");
            var id = int.Parse(Console.ReadLine());
            RemoveEmployee(employeeRepository, id);
            break;
        case "3":
            WriteAllEmployeesToConsole(employeeRepository);
            break;
        case "4":
            Console.Write("give name of boss who you want to add: ");
            var bosName = Console.ReadLine();
            AddBossFromConsole(bossRepository, bosName);
            break;
        case "5":
            Console.Clear();
            Console.WriteLine("!!!!!!!!!! Boss Id: 1, Boss Pasword: lol !!!!!!!!!!!!!!!");
            Console.WriteLine(menu);
            break;
        case "6":
            File.Create(@"Entities\Boss.txt").Close();
            File.Create(@"Entities\Employee.txt").Close();
            foreach (var emp in employeeRepository.GetAll())
            {
                ss.AddToFile(emp);
            }
            ss.AuditSaveInFile();
            Console.WriteLine("\nChanges Saved\n");
            break;
        case "7":
            startStop = false;
            // Environment.Exit(0);
            break;
    }
    Console.Write("\nChose: ");
}

static void EmployeeRepositoryFromItemAdded(object? sender, Employee e)
{
    Console.WriteLine($"\n{e.GetType().Name} added {e.FirstName} from {sender?.GetType().Name}\n");
    SaveToFile.AuditSaveToMemory(e,"added",e.FirstName);
}

static void EmployeeRepositoryFromItemRemove(object? sender, Employee e)
{
    Console.WriteLine($"\n{e.GetType().Name} remove {e.FirstName} from {sender?.GetType().Name}\n");
    SaveToFile.AuditSaveToMemory(e, "remove", e.FirstName);
}

static void AddEmployees(IRepository<Employee> employeeRepository)
{
    var employees = new[]
    {
        new Employee { FirstName = "Mikołaj", Password = "lol" },
        new Employee { FirstName = "Ala" },
        new Employee { FirstName = "Tomek" }
    };
    
    employeeRepository.AddBatch(employees);
}

static void AddBoss(IWriteRepository<Boss> bossRepository)
{
    bossRepository.Add(new Boss { FirstName = "Natka", Password = "lol" });
    bossRepository.Save();
}

static void WriteAllEmployeesToConsole(IReadRepository<IEntity> employeeRepository)
{
    var employees = employeeRepository.GetAll();
    employees = employees.OrderBy(o => o.Id);
    Console.WriteLine("\n***EMPLOYEES***");
    foreach (var emp in employees)
    {
        Console.WriteLine(emp);
    }
}

static void AddEmployeesFromConsole(IRepository<Employee> employeeRepository, string name)
{
    if (CheckAccess(employeeRepository, typeof(Boss)) == true)
    {
        employeeRepository.Add(new Employee { FirstName = name });
        employeeRepository.Save();
    }
}

static void AddBossFromConsole(IRepository<Boss> bossRepository, string name)
{
    if (CheckAccess(bossRepository, typeof(Boss)) == true)
    {
        bossRepository.Add(new Boss { FirstName = name });
        bossRepository.Save();
    }
}

static void RemoveEmployee(IRepository<Employee> employeeRepository, int id)
{
    if(CheckAccess(employeeRepository, typeof(Boss)) == true)
    {
        var emp = employeeRepository.GetById(id);
        employeeRepository.Remove(emp);
        employeeRepository.Save();
    }
}

static bool CheckAccess(IReadRepository<IEntity> employeeRepository, Type type)
{
    Console.Write("Your Id: ");
    var empId = Console.ReadLine();
    Console.Write("Your Password: ");
    var empPassword = Console.ReadLine();

    var emp = employeeRepository.GetById(int.Parse(empId));
    if (emp.Password == empPassword & emp.GetType() == type)
    {
        return true;
    }
    else
    {
        return false;
    }   
}


