using Microsoft.EntityFrameworkCore;
using System.Composition;
using ToDoApp.Data;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ToDoAppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoggerService(ToDoAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task CreateAsync(string message, string user)
        {
            Logger logger = new()
            {
                Message = message,
                Author = user
            };
            
            _context.Loggers.Add(logger);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Logger>> GetAllAsync(string sortOrder, string searchString, int? pageNumber)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }

            IQueryable<Logger> loggers = from e in _context.Loggers
                                     select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                loggers = loggers.Where(e => e.Message.Contains(searchString)
                                          || e.ActionDateTime.ToString().Contains(searchString)
                                          || e.Author.Contains(searchString));
            }

            loggers = sortOrder switch
            {
                "message" => loggers.OrderBy(e => e.Message),
                "message_desc" => loggers.OrderByDescending(e => e.Message),
                "date" => loggers.OrderBy(e => e.ActionDateTime),
                "date_desc" => loggers.OrderByDescending(e => e.ActionDateTime),
                "author" => loggers.OrderBy(e => e.Author),
                "author_desc" => loggers.OrderByDescending(e => e.Author),
                _ => loggers.OrderBy(e => e.Id),
            };

            int pageSize = Int32.Parse(_configuration.GetSection("PageSizes").GetSection("Logger").Value);
            return await PaginatedList<Logger>.CreateAsync(loggers.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task ExportAsync()
        {

        }
    }
}
