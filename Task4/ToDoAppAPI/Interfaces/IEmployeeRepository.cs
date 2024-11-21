using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(string id);
        Task<string> UpdateAsync(string id, UpdateEmployeeDto updateEmployeeDto);
        Task<string> DeleteAsync(string id);
        Task<string> ValidateUser(LoginDto loginDto);
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<string> ChangeEmailAsync(ChangeEmailDto changeEmailDto);
        Task<string> ChangeUserNameAsync(ChangeUserNameDto changeUserNameDto);        
        Task<string> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}
