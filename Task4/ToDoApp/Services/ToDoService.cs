using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoAppDbContext _context;
        private readonly IConfiguration _configuration;

        public ToDoService(ToDoAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<ToDo>> GetAllAsync()
        {
            return await _context.ToDos.ToListAsync();
        }
        public async Task<List<ToDo>> GetAllByEmployeeIdAsync(string sortOrder, string searchString, int? pageNumber, Guid id)
        {
            IQueryable<ToDo> toDos = from e in _context.ToDos
                                     where e.EmployeeId.CompareTo(id) == 0
                                     select e;

            return await PreparePaginatedList(sortOrder, searchString, pageNumber, toDos);
        }

        public async Task<List<ToDo>> GetAllAsync(string sortOrder, string searchString, int? pageNumber)
        {
            IQueryable<ToDo> toDos = from e in _context.ToDos
                                     select e;

            return await PreparePaginatedList(sortOrder, searchString, pageNumber, toDos);
        }

        private async Task<List<ToDo>> PreparePaginatedList(string sortOrder, string searchString, int? pageNumber, IQueryable<ToDo> toDos)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
                toDos = toDos.Where(e => e.Name.Contains(searchString)
                                      || e.Description.Contains(searchString));
            }

            toDos = sortOrder switch
            {
                "name" => toDos.OrderBy(e => e.Name),
                "name_desc" => toDos.OrderByDescending(e => e.Name),
                "description" => toDos.OrderBy(e => e.Description),
                "description_desc" => toDos.OrderByDescending(e => e.Description),
                "status" => toDos.OrderBy(e => e.IsClosed),
                "status_desc" => toDos.OrderByDescending(e => e.IsClosed),
                _ => toDos.OrderBy(e => e.Id),
            };

            int pageSize = Int32.Parse(_configuration.GetSection("PageSizes").GetSection("ToDo").Value);
            return await PaginatedList<ToDo>.CreateAsync(toDos.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<ToDo> GetByIdAsync(Guid id)
        {
            return await _context.ToDos.FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            ToDo toDo = await _context.ToDos.FindAsync(id);
            if (toDo != null)
            {
                _context.ToDos.Remove(toDo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetIdsByEmployeeIdAsync(Guid id)
        {
            List<Guid> ids = [];

            IQueryable<ToDo> toDos = _context.ToDos.Where(e => e.EmployeeId.CompareTo(id) == 0);

            foreach (ToDo toDo in toDos)
            {
                ids.Add(toDo.Id);
            }            

            return String.Join(", ", ids);
        }
        public async Task CreateAsync(ToDo toDo)
        {
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToDo toDo)
        {
            _context.Update(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task StatusChangeAsync(Guid id)
        {
            ToDo toDo = await _context.ToDos.FindAsync(id);
            if (toDo != null)
            {
                toDo.IsClosed = !toDo.IsClosed;
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task DuplicateAsync(Guid id)
        {
            ToDo toDo = await _context.ToDos.FindAsync(id);
            if (toDo != null)
            {
                toDo.Id = new Guid();
                _context.ToDos.Add(toDo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Guid> GetEmployeeIdAsync(Guid id)
        {
            ToDo toDo = await _context.ToDos.FindAsync(id);

            Guid employeeId = new();

            if (toDo != null)
            {
                employeeId = toDo.EmployeeId;
            }

            return employeeId;
        }
    }
}
