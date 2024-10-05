using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models
{
    public class LoggerDbContext(DbContextOptions<LoggerDbContext> options) : DbContext(options)
    {
        public DbSet<Logger> Loggers { get; set; }

    }
}
