using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Model.Data
{
    internal class AppContext : DbContext
    {
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ManageCitizens;Trusted_Connection=True;");
        }

    }
}
