using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Data;
using ToDoAppAPI.Dtos.ToDo;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Repositories
{
    public class ToDoRepository(ApplicationDbContext context) : IToDoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<ToDo>> GetAllAsync()
        {
            return await _context.ToDos.ToListAsync();
        }

        public async Task<ToDo?> GetByIdAsync(string id)
        {
            return await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(ToDo toDo)
        {
            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateAsync(string id, ToDo toDo)
        {
            var existingToDo = await _context.ToDos.FindAsync(id);

            if (existingToDo is null) return $"ToDo with id '{id}' not found";            

            existingToDo.Name = toDo.Name;
            existingToDo.Description = toDo.Description;
            existingToDo.IsClosed = toDo.IsClosed;

            await _context.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var toDo = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);

            if (toDo is null) return $"ToDo with id '{id}' not found";
            
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(string id)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ToDo>> GetAllByEmployeeIdAsync(string id)
        {
            var toDos = _context.ToDos.Where(x => x.EmployeeId == id);

            return await toDos.ToListAsync();
        }

        public async Task<string> DuplicateAsync(string id)
        {
            var toDo = await _context.ToDos.FindAsync(id);

            if (toDo is null) return $"ToDo with id '{id}' not found";
            
            ToDo duplicateToDo = new()
            {
                Name = toDo.Name,
                Description = toDo.Description,
                IsClosed = toDo.IsClosed,
                EmployeeId = toDo.EmployeeId
            };

            await _context.ToDos.AddAsync(duplicateToDo);
            await _context.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<string> StatusChangeAsync(string id)
        {
            var toDo = await _context.ToDos.FindAsync(id);

            if (toDo is null) return $"ToDo with id '{id}' not found";
            
            toDo.IsClosed = !toDo.IsClosed;

            await _context.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<string> ReassignAsync(ReassignDto reassignDto)
        {
            var toDo = await _context.ToDos.FindAsync(reassignDto.ToDoId);

            if (toDo is null) return $"ToDo with id {reassignDto.ToDoId} not found";

            var employee = await _context.Employees.FindAsync(reassignDto.NewEmployeeId);

            if (employee is null) return $"Employee with id {reassignDto.NewEmployeeId} not found";

            toDo.EmployeeId = employee.Id;

            await _context.SaveChangesAsync();

            return string.Empty;
        }
    }
}
