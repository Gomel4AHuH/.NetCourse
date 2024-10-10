using ToDoApp.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task CreateAsync(EmployeeVM employeeVM);
        Task UpdateAsync(EmployeeVM employeeVM);
        Task DeleteAsync(int id);
        Task CreateToDoAsync(ToDo toDo, int id);
    }
}
