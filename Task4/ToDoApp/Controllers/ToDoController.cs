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

        #region API
        [HttpGet]
        [Route("GetAllToDos")]
        public List<ToDo> GetAll()
        {
            return [.. _toDoDbContext.ToDos];
        }

        [HttpPost]
        [Route("AddToDo")]
        public string AddToDo(ToDo toDo)
        {
            _toDoDbContext.ToDos.Add(toDo);
            _toDoDbContext.SaveChanges();
            return "ToDo added.";
        }

        [HttpGet]
        [Route("GetToDo")]
        public ToDo GetToDo(int id)
        {
            return _toDoDbContext.ToDos.Where(toDo => toDo.Id == id).FirstOrDefault();
        }

        [HttpPost]
        [Route("UpdateToDo")]
        public string UpdateToDo(ToDo toDo)
        {
            _toDoDbContext.Entry(toDo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _toDoDbContext.SaveChanges();
            return "ToDo updated.";
        }

        [HttpDelete]
        [Route("DeleteToDo")]
        public string DeleteToDo(int id)
        {
            ToDo toDo = _toDoDbContext.ToDos.Where(toDo => toDo.Id == id).FirstOrDefault();
            if (toDo != null)
            {
                _toDoDbContext.ToDos.Remove(toDo);
                _toDoDbContext.SaveChanges();
                return "ToDo deleted.";
            }
            else
            {
                return "ToDo not found.";
            }
        }
        #endregion

        #region Actions
        // GET: ToDoController
        public ActionResult Index()
        {
            List<ToDo> toDoList = [.. _toDoDbContext.ToDos];
            return View(toDoList);
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
            ToDo toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
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
            ToDo toDo = _toDoDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            return View(toDo);
        }

        // POST: ToDoController/Delete/5
        [HttpPost]
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
        #endregion
    }
}
