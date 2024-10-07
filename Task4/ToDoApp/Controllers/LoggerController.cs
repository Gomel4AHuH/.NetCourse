using Microsoft.AspNetCore.Mvc;
using ToDoApp.Data;

namespace ToDoApp.Controllers
{
    public class LoggerController : Controller
    {
        private readonly ToDoAppDbContext _context;

        public LoggerController(ToDoAppDbContext loggerDbContext)
        {
            _context = loggerDbContext;
        }

        // GET: LoggerController
        public ActionResult Index()
        {
            return View();
        }        
    }
}
