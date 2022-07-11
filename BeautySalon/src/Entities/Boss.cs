using BeautySalon.Repositories;

namespace BeautySalon.Entities
{
    public class Boss : Employee
    {
        public override string ToString() => base.ToString() + " (Boss)";
    }
}
