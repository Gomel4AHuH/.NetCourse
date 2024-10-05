using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class LoggerController : Controller
    {
        private readonly LoggerDbContext _loggerDbContext;

        public LoggerController(LoggerDbContext loggerDbContext)
        {
            _loggerDbContext = loggerDbContext;
        }

        // GET: LoggerController
        public ActionResult Index()
        {
            return View();
        }        
    }
}
