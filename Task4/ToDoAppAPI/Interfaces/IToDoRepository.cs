using ToDoAppAPI.Dtos.ToDo;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Interfaces
{
    public interface IToDoRepository
    {
        Task<List<ToDo>> GetAllAsync();
        Task<ToDo?> GetByIdAsync(string id);
        Task<ToDo> CreateAsync(ToDo toDo);
        Task<ToDo?> UpdateAsync(string id, ToDo ToDo);
        Task<ToDo?> DeleteAsync(string id);
        Task<Employee?> GetEmployeeByIdAsync(string id);
        Task<List<ToDo>> GetAllByEmployeeIdAsync(string id);
        Task<ToDo?> DuplicateAsync(string id);
        Task<ToDo?> StatusChangeAsync(string id);
        Task<string> ReassignAsync(ReassignDto reassignDto);
    }
}
