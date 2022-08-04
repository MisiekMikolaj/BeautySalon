using BeautySalon.Entities.Stuff;
using BeautySalon.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Data
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


        /*public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Boss> Bosses => Set<Boss>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("StorageAppDb");
        }*/
    }
}
