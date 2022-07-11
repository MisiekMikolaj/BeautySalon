using BeautySalon.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Data
{
    public class BeautySalonDbContext : DbContext
    {
        /*public BeautySalonDbContext(DbContextOptions<BeautySalonDbContext> options) 
            : base(options)
        {
        }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }*/
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Boss> Bosses => Set<Boss>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("StorageAppDb");
        }
    }
}
