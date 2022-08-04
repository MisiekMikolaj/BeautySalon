using BeautySalon.Data;
using BeautySalon.Entities.Users;

namespace BeautySalon.Entities.Stuff
{
    public class WorkSchedule : IEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Date { get; set; }
        public ICollection<Day> Days { get; set; }

        public WorkSchedule()
        {
            Days = new List<Day>();
        }
        public override string ToString() => $"    Week schedule: {Date}";


    }
}
