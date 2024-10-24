using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Data;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _context;
        public ToDoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ToDo>> GetAllAsync()
        {
            var toDos = _context.ToDos;

            return await toDos.ToListAsync();
        }

        public async Task<ToDo?> GetByIdAsync(string id)
        {
            return await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ToDo> CreateAsync(ToDo toDo)
        {
            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
            return toDo;
        }

        public async Task<ToDo?> UpdateAsync(string id, ToDo toDo)
        {
            var existingToDo = await _context.ToDos.FindAsync(id);

            if (existingToDo == null)
            {
                return null;
            }

            existingToDo.Name = toDo.Name;
            existingToDo.Description = toDo.Description;
            existingToDo.IsClosed = toDo.IsClosed;

            await _context.SaveChangesAsync();

            return existingToDo;
        }

        public async Task<ToDo?> DeleteAsync(string id)
        {
            var toDo = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);

            if (toDo == null)
            {
                return null;
            }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            return toDo;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(string id)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
