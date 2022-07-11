namespace BeautySalon.Entities
{
    public class Client : EntityBase
    {
        public string? Email { get; set; }
        public override string ToString() => $"Id: {Id},  Email: {Email}";
        
    }
}
