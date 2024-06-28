using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Models.Data
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Citizen> Citizens { get; set; } = null!;

        public ApplicationDbContext()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ManageCitizens;Trusted_Connection=True;");
        }

    }
}
