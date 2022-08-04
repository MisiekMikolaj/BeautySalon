namespace BeautySalon.Entities.Stuff
{
    public class Day : IEntity
    {
        public int Id { get; set; }
        public int WorkScheduleId { get; set; }
        public WorkSchedule WorkSchedule { get; set; }
        public bool IsToday { get; set; }
        public string Date { get; set; }
        public string? Name { get; set; }
        public IList<Houer> Houers { get; set; }
        public Day()
        {
            List<Houer> Houers = new List<Houer>();
        }
        public override string ToString() => $"        {Name}   {Date}\n        Is Today at work: {IsToday}";

    }
}
