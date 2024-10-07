using ToDoApp.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.Interfaces

{
    public interface ILoggerService
    {        
        Task CreateAsync(Logger logger);
    }
}
