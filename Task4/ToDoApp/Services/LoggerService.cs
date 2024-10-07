using ToDoApp.Data;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ToDoAppDbContext _context;

        public LoggerService(ToDoAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Logger logger)
        {
            _context.Loggers.Add(logger);
            await _context.SaveChangesAsync();
        }
    }
}
