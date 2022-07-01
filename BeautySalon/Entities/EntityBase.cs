namespace BeautySalon.Entities
{
    public abstract class EntityBase : IEntity
    {
        public int Id { get; set; }
        public string? Password { get; set; }
    }
}
