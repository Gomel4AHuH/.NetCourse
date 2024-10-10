using Microsoft.AspNetCore.Mvc;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDoService _toDoService;
        private readonly ILoggerService _logger;
        private string Message;

        public ToDoController(IToDoService toDoService, ILoggerService logger)
        {
            _toDoService = toDoService;
            _logger = logger;            
        }

        #region API
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAll()
        {
            return await _toDoService.GetAllAsync();
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<ToDo>> GetToDo(int id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);

            if (toDo == null)
            {
                return NotFound("ToDo not found.");
            }

            return toDo;
        }

        /*[HttpPost]
        [Route("Add")]
        public async Task<ActionResult<ToDo>> AddToDo(ToDo toDo)
        {
            await _toDoService.CreateAsync(toDo);
            return NoContent();
        }*/

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateToDo(int id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);

            if (toDo == null)
            {
                return NotFound("ToDo not found.");
            }

            //await _toDoService.UpdateAsync(toDo);

            return NoContent();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }

            await _toDoService.DeleteAsync(id);

            return NoContent();
        }
        #endregion

        #region Actions
        // GET: ToDoController
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["IdSortParm"] = sortOrder == "id" ? "id" : "";
                ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
                ViewData["DescriptionSortParm"] = sortOrder == "description" ? "description_desc" : "description";
                ViewData["EmployeeIdSortParm"] = sortOrder == "employeeId" ? "employeeId_desc" : "employeeId";

                ViewData["CurrentFilter"] = searchString;

                List<ToDo> toDoList = await _toDoService.GetAllAsync(sortOrder, searchString, pageNumber);

                if (toDoList.Count == 0)
                {
                    Message = "No todos available for now.";
                    TempData["InfoMessage"] = Message;
                }

                return View(toDoList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }
        
        // GET: ToDoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDo toDo)
        {
            try
            {
                await _toDoService.CreateAsync(toDo);
                Message = "ToDo created successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }

        // GET: ToDoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ToDo toDo = await _toDoService.GetByIdAsync(id);
                if (toDo == null)
                {
                    Message = "ToDo details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    await _logger.CreateAsync(Message);
                    return RedirectToAction("Index");
                }
                return View(toDo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }

        // POST: ToDoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ToDo toDo)
        {
            try
            {
                await _toDoService.UpdateAsync(toDo);
                Message = $"ToDo with id {toDo.Id} updated successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }

        // GET: ToDoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                ToDo toDo = await _toDoService.GetByIdAsync(id);
                if (toDo == null)
                {
                    Message = "ToDo details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    await _logger.CreateAsync(Message);
                    return RedirectToAction("Index");
                }
                return View(toDo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }

        // GET: ToDoController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);
            return View(toDo);
        }

        // POST: ToDoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _toDoService.DeleteAsync(id);
                Message = $"ToDo with id {id} deleted successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }

        // GET: ToDoController/Close
        public async Task<IActionResult> Close(int id)
        {
            try
            {
                await _toDoService.CloseAsync(id);
                Message = $"ToDo with id {id} closed successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }

        // GET: ToDoController/Close
        public async Task<IActionResult> Open(int id)
        {
            try
            {
                await _toDoService.OpenAsync(id);
                Message = $"ToDo with id {id} opened successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }

        // GET: ToDoController/Duplicate
        public async Task<IActionResult> Duplicate(int id)
        {
            try
            {
                await _toDoService.DuplicateAsync(id);
                Message = $"ToDo with id {id} duplicated successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }
        #endregion
    }
}
