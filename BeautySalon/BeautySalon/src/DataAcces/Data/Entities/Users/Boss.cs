using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.DataAcces.Data.Entities.Users
{

    public class Boss : Employee
    {

        public override string ToString() => base.ToString() + " (Boss)";
    }
}
