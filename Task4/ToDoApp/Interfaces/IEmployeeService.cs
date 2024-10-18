using ToDoApp.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(Guid id);
        Task CreateAsync(EmployeeVM employeeVM);
        Task UpdateAsync(EmployeeVM employeeVM, Employee employee);
        Task DeleteAsync(Guid id);
        Task CreateToDoAsync(ToDo toDo, Guid id);                
        EmployeeVM EmployeeToEmployeeVM(Employee employee);
    }
}
