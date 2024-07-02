using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Citizen> Citizens { get; set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ManageCitizens;Trusted_Connection=True;");
        }

    }
}
