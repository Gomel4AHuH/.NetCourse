using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models
{
    public class ToDoDbContext(DbContextOptions<ToDoDbContext> options) : DbContext(options)
    {
        public DbSet<ToDo> ToDos { get; set; }

    }
}
