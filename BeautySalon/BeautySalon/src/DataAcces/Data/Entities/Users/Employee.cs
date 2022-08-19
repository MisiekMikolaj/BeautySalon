using BeautySalon.DataAcces.Data.Entities.Stuff;

namespace BeautySalon.DataAcces.Data.Entities.Users
{

    public class Employee : UserEntityBase
    {
        public string? FirstName { get; set; }
        public ICollection<WorkSchedule> WorkSchedules { get; set; }
        public Employee()
        {
            WorkSchedules = new List<WorkSchedule>();
        }

        public override string ToString() => $"Employee Id: {Id},  First name: {FirstName}";

    }
}

