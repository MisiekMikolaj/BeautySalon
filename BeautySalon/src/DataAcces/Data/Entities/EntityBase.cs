using System.Text.Json.Serialization;

namespace BeautySalon.DataAcces.Data.Entities
{
    public class EntityBase : IEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
