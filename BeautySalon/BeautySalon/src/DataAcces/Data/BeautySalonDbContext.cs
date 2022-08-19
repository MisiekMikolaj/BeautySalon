using BeautySalon.DataAcces.Data.Entities.Stuff;
using BeautySalon.DataAcces.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.DataAcces.Data
{
    public class BeautySalonDbContext : DbContext
    {
        public BeautySalonDbContext(DbContextOptions<BeautySalonDbContext> options) 
            : base(options)
        {
        }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Boss> Bosses { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Houer> Houers { get; set; }

    }
}
