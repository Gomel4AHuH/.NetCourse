using ToDoApp.Dtos.Employee;
using ToDoApp.Dtos.Employee.Authorization;
using ToDoApp.Models;

namespace ToDoApp.Interfaces
{
    public interface IEmployeeService
    {
        Task<HttpResponseMessage> LoginAsync(LoginDto loginModel);
        Task<HttpResponseMessage> LogoutAsync();
        Task<HttpResponseMessage> RegisterAsync(RegisterDto registerModel);
        Task<List<Employee>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(Guid id);
        Task<HttpResponseMessage> UpdateAsync(EditEmployeeDto editEmployeeDto, Employee employee);
        Task<HttpResponseMessage> DeleteAsync(Guid id);
        Task<HttpResponseMessage> ChangeEmailAsync(ChangeEmailDto changeEmailDto);
        Task<HttpResponseMessage> ChangeUserNameAsync(ChangeUserNameDto changeUserNameDto);
        Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    }
}
