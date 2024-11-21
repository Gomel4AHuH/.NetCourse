using ToDoApp.Models;
using ToDoApp.Dtos.ToDo;

namespace ToDoApp.Interfaces
{
    public interface IToDoService
    {
        Task<List<ToDo>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task<List<ToDo>> GetAllByEmployeeIdAsync(string sortOrder, string searchString, int? pageNumber, Guid id);
        Task<ToDo> GetByIdAsync(Guid id);
        Task<HttpResponseMessage> CreateAsync(CreateToDoDto createToDo);
        Task<HttpResponseMessage> UpdateAsync(ToDo toDo);
        Task<HttpResponseMessage> DeleteAsync(Guid id);        
        Task<List<ToDo>> GetAllByEmployeeIdAsync(Guid id);
        Task<HttpResponseMessage> DuplicateAsync(Guid id);
        Task<HttpResponseMessage> StatusChangeAsync(Guid id);
        Task<HttpResponseMessage> ReassignAsync(ReassignDto reassignDto);
    }
}
