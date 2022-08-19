namespace BeautySalon.DataAcces.Data.Entities.Users
{
    public class Client : UserEntityBase
    {
        public string? Email { get; set; }
        public override string ToString() => $"Id: {Id},  Email: {Email}";

    }
}
