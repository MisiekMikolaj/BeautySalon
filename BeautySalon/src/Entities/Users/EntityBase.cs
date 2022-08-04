namespace BeautySalon.Entities.Users
{
    public class EntityBase : IEntity
    {
        public int Id { get; set; }
        public string? Password { get; set; }
    }
}
