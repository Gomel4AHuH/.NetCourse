using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoDbContext _toDoDbContext;

        public ToDoController(ToDoDbContext toDoDbContext)
        {
            _toDoDbContext = toDoDbContext;
        }

        // GET: ToDoController
        public ActionResult Index()
        {
            List<ToDo> toDoList = [.. _toDoDbContext.ToDos];
            return View(toDoList);
        }

        [HttpGet]
        [Route("GetAllToDos")]
        public List<ToDo> GetAll()
        {
            return [.. _toDoDbContext.ToDos];
        }

        // GET: ToDoController/Details/5
        public ActionResult Details(int id)
        {
            ToDo toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            return View(toDo);
        }

        // GET: ToDoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoController/Create
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ToDo toDo)
        {
            try
            {
                _toDoDbContext.ToDos.Add(toDo);
                _toDoDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Route("GetToDo")]
        public ToDo GetToDo(int id)
        {
            return _toDoDbContext.ToDos.Where(toDo => toDo.Id == id).FirstOrDefault();
        }

        // GET: ToDoController/Edit/5
        public ActionResult Edit(int id)
        {
            ToDo toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            return View(toDo);
        }

        // POST: ToDoController/Edit/5
        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ToDo toDo)
        {
            try
            {
                _toDoDbContext.ToDos.Add(toDo);
                _toDoDbContext.Entry(toDo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _toDoDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ToDoController/Delete/5
        public ActionResult Delete(int id)
        {
            ToDo toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            return View(toDo);
        }

        // POST: ToDoController/Delete/5
        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                ToDo toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
                if (toDo != null)
                {
                    _toDoDbContext.Remove(toDo);
                    _toDoDbContext.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
