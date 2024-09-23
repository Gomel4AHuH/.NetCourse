using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models
{
    public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }

    }
}
