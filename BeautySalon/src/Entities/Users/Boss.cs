using BeautySalon.Repositories;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Entities.Users
{

    public class Boss : Employee
    {

        public override string ToString() => base.ToString() + " (Boss)";
    }
}
