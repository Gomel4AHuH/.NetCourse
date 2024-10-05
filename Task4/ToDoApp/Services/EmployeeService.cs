using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeDbContext _employeeContext;

        public EmployeeService(EmployeeDbContext context)
        {
            _employeeContext = context;
        }

        public async Task<List<Employee>> GetAllAsync(string searchString, string sortOrder)
        {
            //List<Employee> employees = [.. _employeeContext.Employees];
            IQueryable<Employee> employees = from e in _employeeContext.Employees
                                       select e;

            switch (sortOrder)
            {
                case "id":
                    employees = employees.OrderBy(e => e.Id);
                    break;
                case "lastName":
                    employees = employees.OrderBy(e => e.LastName);
                    break;
                case "lastName_desc":
                    employees = employees.OrderByDescending(e => e.LastName);
                    break;
                case "firstName":
                    employees = employees.OrderBy(e => e.FirstName);
                    break;
                case "firstName_desc":
                    employees = employees.OrderByDescending(e => e.FirstName);
                    break;
                case "middleName":
                    employees = employees.OrderBy(e => e.MiddleName);
                    break;
                case "middleName_desc":
                    employees = employees.OrderByDescending(e => e.MiddleName);
                    break;
                case "birthday":
                    employees = employees.OrderBy(e => e.Birthday);
                    break;
                case "birthday_desc":
                    employees = employees.OrderByDescending(e => e.Birthday);
                    break;
                case "speciality":
                    employees = employees.OrderBy(e => e.Speciality);
                    break;
                case "speciality_desc":
                    employees = employees.OrderByDescending(e => e.Speciality);
                    break;
                case "employmentDate":
                    employees = employees.OrderBy(e => e.EmploymentDate);
                    break;
                case "employmentDate_desc":
                    employees = employees.OrderByDescending(e => e.EmploymentDate);
                    break;
                default:
                    employees = employees.OrderBy(e => e.Id);
                    break;
            }

            return [.. employees];
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _employeeContext.Employees.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            Employee employee = await _employeeContext.Employees.FindAsync(id);
            if (employee != null)
            {
                _employeeContext.Employees.Remove(employee);
                await _employeeContext.SaveChangesAsync();
            }
        }
        public async Task CreateAsync(Employee employee)
        {
            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();
        }
    }    
}
