using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces;
using ToDoApp.Models;
using ToDoApp.Data;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace ToDoApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ToDoAppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EmployeeService(ToDoAppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }
        public async Task<List<Employee>> GetAllAsync(string sortOrder, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            IQueryable<Employee> employees = from e in _context.Employees
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

            int pageSize = 10;
            return await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            Employee employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                DeletePhoto(employee.EmployeePhotoPath);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        private void DeletePhoto(string fileName)
        {
            string oldFullPath = _environment.WebRootPath + "/photos/" + fileName;
            System.IO.File.Delete(oldFullPath);
        }

        private string CreatePhoto(EmployeeVM employeeVM)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            fileName += Path.GetExtension(employeeVM.EmployeePhoto.FileName);

            string fullPath = _environment.WebRootPath + "/photos/" + fileName;
            using (var stream = System.IO.File.Create(fullPath))
            {
                employeeVM.EmployeePhoto.CopyTo(stream);
            }

            return fileName;
        }

        public async Task CreateAsync(EmployeeVM employeeVM)
        {
            _context.Employees.Add(EmployeeVMToEmployee(employeeVM));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployeeVM employeeVM, Employee employee)
        {
            string fileName = employee.EmployeePhotoPath;
            if (employeeVM.EmployeePhoto != null)
            {
                DeletePhoto(fileName);
                fileName = CreatePhoto(employeeVM);
            }

            employee.LastName = employeeVM.LastName;
            employee.FirstName = employeeVM.FirstName;
            employee.MiddleName = employeeVM.MiddleName;
            employee.Birthday = employeeVM.Birthday;
            employee.Speciality = employeeVM.Speciality;
            employee.EmploymentDate = employeeVM.EmploymentDate;
            employee.EmployeePhotoPath = fileName;

            await _context.SaveChangesAsync();
        }

        public async Task CreateToDoAsync(ToDo toDo, int id)
        {
            toDo.EmployeeId = id;
            toDo.Id = 0;
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
        }

        private Employee EmployeeVMToEmployee(EmployeeVM employeeVM)
        {
            string fileName = CreatePhoto(employeeVM);            

            Employee employee = new()
            {
                LastName = employeeVM.LastName,
                FirstName = employeeVM.FirstName,
                MiddleName = employeeVM.MiddleName,
                Birthday = employeeVM.Birthday,
                Speciality = employeeVM.Speciality,
                EmploymentDate = employeeVM.EmploymentDate,
                EmployeePhotoPath = fileName
            };

            return employee;
        }

        public EmployeeVM EmployeeToEmployeeVM(Employee employee)
        {

            EmployeeVM employeeVM = new()
            {
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                Birthday = employee.Birthday,
                Speciality = employee.Speciality,
                EmploymentDate = employee.EmploymentDate
            };

            return employeeVM;
        }
    }    
}
