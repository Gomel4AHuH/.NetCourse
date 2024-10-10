using ToDoApp.Models;

namespace ToDoApp.Interfaces
{
    public interface IToDoService
    {
        Task<List<ToDo>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task<List<ToDo>> GetAllAsync();
        Task<ToDo> GetByIdAsync(int id);
        Task CreateAsync(ToDo toDo);
        Task UpdateAsync(ToDo toDo);
        Task DeleteAsync(int id);
        Task CloseAsync(int id);
        Task OpenAsync(int id);
        Task DuplicateAsync(int id);
    }
}
