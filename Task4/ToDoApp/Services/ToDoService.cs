using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoDbContext _toDoContext;

        public ToDoService(ToDoDbContext context)
        {
            _toDoContext = context;
        }

        /*
        public async Task<Models.ToDo?> GetByIdAsync(int id)
        {
            return await _toDoContext.ToDos
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateAsync(Models.ToDo toDo)
        {
            _toDoContext.ToDos.Add(toDo);
            await _toDoContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.ToDo task)
        {
            _toDoContext.ToDos.Update(task);
            await _toDoContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _toDoContext.ToDos.FindAsync(id);
            if (task != null)
            {
                _toDoContext.ToDos.Remove(task);
                await _toDoContext.SaveChangesAsync();
            }
        }*/
    }
}
