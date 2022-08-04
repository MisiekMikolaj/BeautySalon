using BeautySalon.Data;
using BeautySalon.Entities.Stuff;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Entities.Users
{

    public class Employee : EntityBase
    {
        public string? FirstName { get; set; }
        public ICollection<WorkSchedule> WorkSchedules { get; set; }
        public Employee()
        {
            WorkSchedules = new List<WorkSchedule>();
        }

        public override string ToString() => $"Employee Id: {Id},  First name: {FirstName}";

        public void cos()
        {

            WorkSchedules.Add(new WorkSchedule() { Date = "12-15", Days = new List<Day> { new Day { IsToday = false, Name = "poniedziałek", Date = "12.01", Houers = new List<Houer> { new Houer { Time = "8:00", Free = false }, new Houer { Time = "9:00", Free = false } } } } });
        }
    }
}
