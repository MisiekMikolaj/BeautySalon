namespace BeautySalon.DataAcces.Data.Entities.Stuff
{
    public class Houer : EntityBase
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; }
        public string Time { get; set; }
        public string? Client { get; set; }
        public bool IsFree { get; set; }
        public string? Service { get; set; }

        public override string ToString() => $"            Time : {Time}    Is worker free: {IsFree}    Client: {Client}    Service: {Service}";
    }
}
