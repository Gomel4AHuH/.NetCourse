using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<ToDo> ToDos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles =
            [
                new() {
                    Name = "Manager",
                    NormalizedName = "Manager"
                },
                new() {
                    Name = "Employee",
                    NormalizedName = "Employee"
                }
            ];
            /*
            List<IdentityUser> users =
            [
                new() {
                    UserName = "Manager",
                    Email = "manager@gmail.com"
                }
            ];*/
            
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            //modelBuilder.Entity<IdentityUser>().HasData(users);
        }
    }
}
