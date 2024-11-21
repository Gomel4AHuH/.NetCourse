using ToDoAppAPI.Dtos.ToDo;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Interfaces
{
    public interface IToDoRepository
    {
        Task<List<ToDo>> GetAllAsync();
        Task<ToDo?> GetByIdAsync(string id);
        Task CreateAsync(ToDo toDo);
        Task<string> UpdateAsync(string id, ToDo ToDo);
        Task<string> DeleteAsync(string id);
        Task<Employee?> GetEmployeeByIdAsync(string id);
        Task<List<ToDo>> GetAllByEmployeeIdAsync(string id);
        Task<string> DuplicateAsync(string id);
        Task<string> StatusChangeAsync(string id);
        Task<string> ReassignAsync(ReassignDto reassignDto);
    }
}
