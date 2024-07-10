using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Citizen> Citizens { get; set; }
        public string _sqlDbConnection;

        public ApplicationDbContext()
        {
            _sqlDbConnection = new Configuration().GetConfigurationString();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_sqlDbConnection);
        }

    }
}
