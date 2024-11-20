using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Data;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Dtos.ToDo;
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

        public async Task<List<ToDo>> GetAllByEmployeeIdAsync(string id)
        {
            var toDos = _context.ToDos.Where(x => x.EmployeeId == id);

            return await toDos.ToListAsync();
        }

        public async Task<ToDo?> DuplicateAsync(string id)
        {
            var toDo = await _context.ToDos.FindAsync(id);

            if (toDo == null)
            {
                return null;
            }

            ToDo duplicateToDo = new()
            {
                Name = toDo.Name,
                Description = toDo.Description,
                IsClosed = toDo.IsClosed,
                EmployeeId = toDo.EmployeeId
            };

            await _context.ToDos.AddAsync(duplicateToDo);
            await _context.SaveChangesAsync();

            return duplicateToDo;
        }

        public async Task<ToDo?> StatusChangeAsync(string id)
        {
            var toDo = await _context.ToDos.FindAsync(id);

            if (toDo == null)
            {
                return null;
            }

            toDo.IsClosed = !toDo.IsClosed;

            await _context.SaveChangesAsync();

            return toDo;
        }

        public async Task<string> ReassignAsync(ReassignDto reassignDto)
        {
            var toDo = await _context.ToDos.FindAsync(reassignDto.ToDoId);

            if (toDo == null) return $"ToDo with id {reassignDto.ToDoId} not found";

            var employee = await _context.Employees.FindAsync(reassignDto.NewEmployeeId);

            if (employee == null) return $"Employee with id {reassignDto.NewEmployeeId} not found";

            toDo.EmployeeId = employee.Id;
            await _context.SaveChangesAsync();

            return "";
        }

        private static string IdentityErrorsToString(IdentityResult result)
        {
            List<IdentityError> errorList = result.Errors.ToList();
            return string.Join(", ", errorList.Select(e => e.Description));
        }
    }
}
