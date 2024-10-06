using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces;
using ToDoApp.Models;
using PagedList;

namespace ToDoApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeDbContext _employeeContext;

        public EmployeeService(EmployeeDbContext context)
        {
            _employeeContext = context;
        }

        public async Task<List<Employee>> GetAllAsync(string sortOrder, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            IQueryable<Employee> employees = from e in _employeeContext.Employees
                                       select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.LastName.Contains(searchString)
                                              || e.FirstName.Contains(searchString)
                                              || e.MiddleName.Contains(searchString)
                                              || e.Birthday.ToString().Contains(searchString)
                                              || e.Speciality.Contains(searchString)
                                              || e.EmploymentDate.ToString().Contains(searchString));
            }

            employees = sortOrder switch
            {
                "id" => employees.OrderBy(e => e.Id),
                "lastName" => employees.OrderBy(e => e.LastName),
                "lastName_desc" => employees.OrderByDescending(e => e.LastName),
                "firstName" => employees.OrderBy(e => e.FirstName),
                "firstName_desc" => employees.OrderByDescending(e => e.FirstName),
                "middleName" => employees.OrderBy(e => e.MiddleName),
                "middleName_desc" => employees.OrderByDescending(e => e.MiddleName),
                "birthday" => employees.OrderBy(e => e.Birthday),
                "birthday_desc" => employees.OrderByDescending(e => e.Birthday),
                "speciality" => employees.OrderBy(e => e.Speciality),
                "speciality_desc" => employees.OrderByDescending(e => e.Speciality),
                "employmentDate" => employees.OrderBy(e => e.EmploymentDate),
                "employmentDate_desc" => employees.OrderByDescending(e => e.EmploymentDate),
                _ => employees.OrderBy(e => e.Id),
            };

            int pageSize = 5;
            return await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize);
            //return [.. employees];
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
