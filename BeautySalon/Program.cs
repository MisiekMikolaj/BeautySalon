using BeautySalon.Data;
using BeautySalon.Entities;
using BeautySalon.Repositories;


var employeeRepository = new SqlRepository<Employee>(new BeautySalonDbContext());
AddEmployees(employeeRepository);
AddBoss(employeeRepository);
WriteAllEmployeesToConsole(employeeRepository);
RemoveEmployee(employeeRepository, 2);
Console.WriteLine("");
WriteAllEmployeesToConsole(employeeRepository);

bool startStop = true;
while(startStop)
{
    Console.WriteLine("\nSelect:\n1) if you want add employee\n2) if you want remove employee\n3) if you want display all employee\n4) if you want add boss\n5) if you want clear console\n6) if you want exit \n");
    Console.Write(": ");
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
            AddBossFromConsole(employeeRepository, bosName);
            break;
        case "5":
            Console.Clear();
            break;
        case "6":
            startStop = false;
            break;
    }
}

static void AddEmployees(IRepository<Employee> employeeRepository)
{
    employeeRepository.Add(new Employee { FirstName = "Mikołaj", Password = "lol" });
    employeeRepository.Add(new Employee { FirstName = "Ala" });
    employeeRepository.Add(new Employee { FirstName = "Tomek" });
    employeeRepository.Save();
}

static void AddBoss(IWriteRepository<Boss> bossRepository)
{
    bossRepository.Add(new Boss { FirstName = "Natka", Password = "lol" });
    bossRepository.Save();
}

static void WriteAllEmployeesToConsole(IReadRepository<IEntity> employeeRepository)
{
    var employees = employeeRepository.GetAll();
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

static void AddBossFromConsole(IRepository<Employee> bossRepository, string name)
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
    var yourId = Console.ReadLine();
    Console.Write("Your Password: ");
    var yourPassword = Console.ReadLine();

    var you = employeeRepository.GetById(int.Parse(yourId));
    if (you.Password == yourPassword & you.GetType() == type)
    {
        return true;
    }
    else
    {
        return false;
    }
}