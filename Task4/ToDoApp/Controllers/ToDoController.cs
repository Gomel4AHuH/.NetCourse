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
            var toDoList = _toDoDbContext.ToDos.ToList();
            return View(toDoList);
        }

        // GET: ToDoController/Details/5
        public ActionResult Details(int id)
        {
            var toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            return View(toDo);
        }

        // GET: ToDoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoController/Create
        [HttpPost]
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

        // GET: ToDoController/Edit/5
        public ActionResult Edit(int id)
        {
            var toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            return View(toDo);
        }

        // POST: ToDoController/Edit/5
        [HttpPost]
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
            var toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            return View(toDo);
        }

        // POST: ToDoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
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
