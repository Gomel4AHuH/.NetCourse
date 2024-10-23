using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(string id);
        Task<Employee?> UpdateAsync(string id, Employee employee);
        Task<Employee?> DeleteAsync(string id);
    }
}
