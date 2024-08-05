using ManageCitizens.Services;
using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Citizen> Citizens { get; set; }
        public string _sqlDbConnection;
        private readonly DefaultDialogService _dialogService = new();

        public ApplicationDbContext()
        {            
            try
            {
                _sqlDbConnection = Configuration.GetConfigurationString();
                Database.EnsureCreated();                
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_sqlDbConnection);
        }        
    }
}
