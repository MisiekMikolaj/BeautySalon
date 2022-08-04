namespace BeautySalon.Entities.Stuff
{
    public class Houer : IEntity
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; }
        public string Time { get; set; }
        public string? Client { get; set; }
        public bool Free { get; set; }
        public string? Service { get; set; }

        public override string ToString() => $"            Time : {Time}    Is worker free: {Free}    Client: {Client}    Service: {Service}";
    }
}
