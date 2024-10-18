using ToDoApp.Models;

namespace ToDoApp.Interfaces
{
    public interface IToDoService
    {
        Task<List<ToDo>> GetAllByEmployeeIdAsync(string sortOrder, string searchString, int? pageNumber, Guid id);
        Task<List<ToDo>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task<List<ToDo>> GetAllAsync();
        Task<ToDo> GetByIdAsync(Guid id);
        Task CreateAsync(ToDo toDo);
        Task UpdateAsync(ToDo toDo);
        Task DeleteAsync(Guid id);
        Task<string> GetIdsByEmployeeIdAsync(Guid id);
        Task StatusChangeAsync(Guid id);        
        Task DuplicateAsync(Guid id);
        Task<Guid> GetEmployeeIdAsync(Guid id);
    }
}
