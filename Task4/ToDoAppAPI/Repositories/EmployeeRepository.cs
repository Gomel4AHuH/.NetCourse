using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Data;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Mappers;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Repositories
{
    public class EmployeeRepository(ApplicationDbContext context, UserManager<Employee> userManager, SignInManager<Employee> signInManager) : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<Employee> _userManager = userManager;
        private readonly SignInManager<Employee> _signInManager = signInManager;

        #region Private methods
        private static string IdentityErrorsToString(IdentityResult result)
        {
            List<IdentityError> errorList = result.Errors.ToList();
            return string.Join(", ", errorList.Select(e => e.Description));
        }

        private static async Task<byte[]> CreatePhoto(IFormFile? EmployeePhoto)
        {
            byte[] result = [];

            if (EmployeePhoto?.Length > 0)
            {
                using var memoryStream = new MemoryStream();

                await EmployeePhoto.CopyToAsync(memoryStream);

                result = memoryStream.ToArray();
            }

            return result;
        }
        private static string CreateRandomPassword(int length = 15)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }

        #endregion

        #region Actions
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            List<string> errors = [];

            Employee employee = registerDto.ToEmployeeDto();

            employee.EmployeePhoto = await CreatePhoto(registerDto.EmployeePhoto);

            IdentityResult createdEmployee = await _userManager.CreateAsync(employee, registerDto.Password);

            if (!createdEmployee.Succeeded) errors.Add(IdentityErrorsToString(createdEmployee));

            IdentityResult roleResult = await _userManager.AddToRoleAsync(employee, "Employee");

            if (!roleResult.Succeeded) errors.Add(IdentityErrorsToString(roleResult));

            return string.Join(", ", errors);
        }
        
        public async Task<List<Employee>> GetAllAsync()
        {
            var employees = _context.Employees;

            return await employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<string> UpdateAsync(string id, UpdateEmployeeDto updateEmployeeDto)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee is null) return $"Employee with id '{id}' not found";

            existingEmployee.FirstName = updateEmployeeDto.FirstName;
            existingEmployee.LastName = updateEmployeeDto.LastName;
            existingEmployee.MiddleName = updateEmployeeDto.MiddleName;
            existingEmployee.Birthday = updateEmployeeDto.Birthday;
            existingEmployee.Speciality = updateEmployeeDto.Speciality;
            existingEmployee.EmploymentDate = updateEmployeeDto.EmploymentDate;
            if (updateEmployeeDto.EmployeePhoto is not null) existingEmployee.EmployeePhoto = await CreatePhoto(updateEmployeeDto.EmployeePhoto);

            await _context.SaveChangesAsync();

            return "";
        }

        public async Task<string> DeleteAsync(string id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee is null) return $"Employee with id {id} not found";

            var toDos = _context.ToDos.Where(x => x.EmployeeId ==  id);

            if (toDos is null) return $"ToDos for employee with id {id} not found";

            foreach (ToDo toDo in toDos)
            {
                _context.ToDos.Remove(toDo);
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return "";
        }

        public async Task<string> ValidateUser(LoginDto loginDto)
        {
            Employee employee = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

            if (employee is null) return $"Employee with email {loginDto.Email} not found";
            
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(employee, loginDto.Password, false);

            if (!result.Succeeded) return $"Password for employee with email {loginDto.Email} is not correct";

            return "";
        }                

        public async Task<string> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            Employee employee = await _userManager.Users.FirstOrDefaultAsync(x => string.Equals(x.Email, changePasswordDto.Email.ToLower()));

            if (employee is null) return $"Employee '{changePasswordDto.Email}' not found.";

            if (!await _signInManager.UserManager.CheckPasswordAsync(employee, changePasswordDto.CurrentPassword)) return "Current password is incorrect";

            if (!(string.Compare(changePasswordDto.NewPassword, changePasswordDto.NewPasswordConfirmation) == 0)) return "Passwords don't match";

            if (string.Compare(changePasswordDto.CurrentPassword, changePasswordDto.NewPasswordConfirmation) == 0) return "Old password and new password must be different. Please try again.";

            IdentityResult result = await _signInManager.UserManager.ChangePasswordAsync(employee, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);                      

            if (!result.Succeeded) return string.Join(", ", result.Errors.ToList().Select(e => e.Description));
            
            return "";
        }

        public async Task<string> ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            Employee currentEmployee = await _userManager.Users.FirstOrDefaultAsync(x => string.Equals(x.Email, changeEmailDto.Email.ToLower()));
            
            if (currentEmployee is null) return $"Employee '{changeEmailDto.Email}' not found.";

            Employee newEmployee = await _userManager.Users.FirstOrDefaultAsync(x => string.Equals(x.Email, changeEmailDto.NewEmail.ToLower()));

            if (newEmployee is not null) return $"Email '{changeEmailDto.NewEmail}' is already taken.";

            IdentityResult result = await _signInManager.UserManager.SetEmailAsync(currentEmployee, changeEmailDto.NewEmail);

            if (!result.Succeeded) return string.Join(", ", result.Errors.ToList().Select(e => e.Description));            

            return "";
        }

        public async Task<string> ChangeUserNameAsync(ChangeUserNameDto changeUserNameDto)
        {
            Employee currentEmployee = await _userManager.Users.FirstOrDefaultAsync(x => string.Equals(x.UserName, changeUserNameDto.UserName.ToLower()));

            if (currentEmployee is null) return $"Employee '{changeUserNameDto.UserName}' not found.";

            Employee newEmployee = await _userManager.Users.FirstOrDefaultAsync(x => string.Equals(x.UserName, changeUserNameDto.NewUserName.ToLower()));

            if (newEmployee is not null) return $"Username '{changeUserNameDto.NewUserName}' is already taken.";

            IdentityResult result = await _signInManager.UserManager.SetUserNameAsync(currentEmployee, changeUserNameDto.NewUserName);

            if (!result.Succeeded) return string.Join(", ", result.Errors.ToList().Select(e => e.Description));

            return "";
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            Employee employee = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email.ToLower());

            if (employee is null) return $"Employee '{forgotPasswordDto.Email}' not found.";

            var token = await _userManager.GeneratePasswordResetTokenAsync(employee);

            string newPassword = CreateRandomPassword();

            IdentityResult result = await _userManager.ResetPasswordAsync(employee, token, newPassword);

            if (!result.Succeeded) return string.Join(", ", result.Errors.ToList().Select(e => e.Description));

            return "";
        }
        #endregion
    }
}
