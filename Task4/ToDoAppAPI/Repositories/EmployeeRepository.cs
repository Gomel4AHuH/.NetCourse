using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Data;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Mappers;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Repositories
{
    public class EmployeeRepository(ApplicationDbContext context, UserManager<Employee> userManager, SignInManager<Employee> signInManager, ITokenService tokenService, IWebHostEnvironment environment, IConfiguration configuration) : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<Employee> _userManager = userManager;
        private readonly SignInManager<Employee> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<Employee>> GetAllAsync()
        {
            var employees = _context.Employees;

            return await employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<LoginedDto?> LoginAsync(LoginDto loginDto)
        {
            Employee employee = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if (employee != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(employee, loginDto.Password, false);
                if (result.Succeeded)
                {
                    return new LoginedDto
                    {
                        UserName = employee.UserName,
                        Email = employee.Email,
                        Token = _tokenService.CreateToken(employee)
                    };
                }
            }

            return null;
        }

        public async Task<NewEmployeeDto?> RegisterAsync(RegisterDto registerDto)
        {
            Employee employee = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                MiddleName = registerDto.MiddleName,
                Birthday = registerDto.Birthday,
                Speciality = registerDto.Speciality,
                EmploymentDate = registerDto.EmploymentDate
            };

            IdentityResult createdEmployee = await _userManager.CreateAsync(employee, registerDto.Password);

            if (createdEmployee.Succeeded)
            {
                IdentityResult roleResult = await _userManager.AddToRoleAsync(employee, "Employee");
                if (roleResult.Succeeded)
                {                    
                    NewEmployeeDto newEmployeeDto = employee.ToNewEmployeeDto();
                    newEmployeeDto.Token = _tokenService.CreateToken(employee);
                    newEmployeeDto.EmployeePhotoPath = CreatePhoto(registerDto);
                    return newEmployeeDto;
                }                
            }

            return null;
        }

        private string CreatePhoto(RegisterDto registerDto)
        {
            if (registerDto.EmployeePhoto != null)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                fileName += Path.GetExtension(registerDto.EmployeePhoto.FileName);

                string fullPath = _environment.ContentRootPath + _configuration["PhotosFolder"] + fileName;
                using (var stream = System.IO.File.Create(fullPath))
                {
                    registerDto.EmployeePhoto.CopyTo(stream);
                }

                return fileName;
            }
            else
            {
                return "";
            }
        }
    }
}
