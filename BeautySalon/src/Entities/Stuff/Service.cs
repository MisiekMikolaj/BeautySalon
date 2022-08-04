namespace BeautySalon.Entities.Stuff
{
    public class Service : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Time { get; set; }
        public int Price { get; set; }

        public override string ToString() => $"Id: {Id},  Name: {Name},\n   Time: {Time} minutes,\n   Price: {Price} $";
    }
}
