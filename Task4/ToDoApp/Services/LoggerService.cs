using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class LoggerService(ToDoAppDbContext context, IConfiguration configuration) : ILoggerService
    {
        private readonly ToDoAppDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        public IQueryable<Logger> loggersSearchResult;

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

            loggersSearchResult = from e in _context.Loggers
                                     select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                loggersSearchResult = loggersSearchResult.Where(e => e.Message.Contains(searchString)
                                                             || e.ActionDateTime.ToString().Contains(searchString)
                                                             || e.Author.Contains(searchString));
            }

            loggersSearchResult = sortOrder switch
            {
                "message" => loggersSearchResult.OrderBy(e => e.Message),
                "message_desc" => loggersSearchResult.OrderByDescending(e => e.Message),
                "date" => loggersSearchResult.OrderBy(e => e.ActionDateTime),
                "date_desc" => loggersSearchResult.OrderByDescending(e => e.ActionDateTime),
                "author" => loggersSearchResult.OrderBy(e => e.Author),
                "author_desc" => loggersSearchResult.OrderByDescending(e => e.Author),
                _ => loggersSearchResult.OrderBy(e => e.Id),
            };

            int pageSize = Int32.Parse(_configuration.GetSection("PageSizes").GetSection("Logger").Value);
            return await PaginatedList<Logger>.CreateAsync(loggersSearchResult.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task ExportAsync(List<Logger> logs)
        {
            try
            {
                //using FileStream? fs = new(fileName, FileMode.OpenOrCreate);
                //await JsonSerializer.SerializeAsync(fs, logs);
            }
            catch (Exception ex)
            {
                //dialogService.ShowMessage(ex.Message);
            }
        }        
    }
}
