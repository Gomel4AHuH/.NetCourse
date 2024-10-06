using ToDoApp.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task<Employee> GetByIdAsync(int id);
        //Task<Employee> GetDetailsAsync(int? id, string searchString, string sortOrder, int? pageNumber);
        Task CreateAsync(Employee employee);
        //Task<Employee> GetForEditAsync(int? id);
        //Task UpdateAsync(Employee employee);
        //Task<Employee> GetForDeleteAsync(int? id);
        Task DeleteAsync(int id);
    }
}
