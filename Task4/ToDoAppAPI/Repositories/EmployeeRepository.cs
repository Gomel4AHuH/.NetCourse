using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Data;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            var employees = _context.Employees;

            return await employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            return await _context.Employees.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Employee?> UpdateAsync(string id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.UserName = employee.UserName;
            existingEmployee.Email = employee.Email;
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.MiddleName = employee.MiddleName;
            existingEmployee.Birthday = employee.Birthday;
            existingEmployee.Speciality = employee.Speciality;
            existingEmployee.EmploymentDate = employee.EmploymentDate;
            existingEmployee.EmployeePhotoPath = employee.EmployeePhotoPath;

            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task<Employee?> DeleteAsync(string id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                return null;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
    }
}
