using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Areas.Identity.Data;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDoService _toDoService;
        private readonly ILoggerService _logger;
        private readonly UserManager<ToDoAppUser> _userManager;
        private string Message;

        public ToDoController(IToDoService toDoService, ILoggerService logger, UserManager<ToDoAppUser> userManager)
        {
            _toDoService = toDoService;
            _logger = logger;
            _userManager = userManager;
        }

        private string GetUserMail()
        {
            string name = _userManager.GetUserName(User);
            return name;
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
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber, int id)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["IdSortParm"] = sortOrder == "id" ? "id" : "";
                ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
                ViewData["DescriptionSortParm"] = sortOrder == "description" ? "description_desc" : "description";
                ViewData["EmployeeIdSortParm"] = sortOrder == "employeeId" ? "employeeId_desc" : "employeeId";
                ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";

                ViewData["CurrentFilter"] = searchString;

                ViewData["Id"] = id;

                List<ToDo> toDoList;

                toDoList = await _toDoService.GetAllAsync(sortOrder, searchString, pageNumber, id);

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
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }
        
        // GET: ToDoController/Create
        public ActionResult Create(int employeeId)
        {            
            return View(new Models.ToDo { Name = "", Description = "", EmployeeId = employeeId });
        }

        // POST: ToDoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDo toDo)
        {
            try
            {
                await _toDoService.CreateAsync(toDo);
                Message = $"ToDo with id {toDo.Id} created successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("Index", "ToDo", new { id = toDo.EmployeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
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
                    await _logger.CreateAsync(Message, GetUserMail().ToString());
                    return RedirectToAction("Index");
                }
                return View(toDo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
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
                await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("Index", "ToDo", new { id = toDo.EmployeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
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
                    await _logger.CreateAsync(Message, GetUserMail().ToString());
                    return RedirectToAction("Index");
                }
                return View(toDo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
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
                int employeeId = await _toDoService.GetEmployeeIdAsync(id);
                await _toDoService.DeleteAsync(id);                
                Message = $"ToDo with id {id} deleted successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("Index", "ToDo", new { id = employeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: ToDoController/Close
        public async Task<IActionResult> StatusChange(int id, bool status)
        {
            try
            {
                await _toDoService.StatusChangeAsync(id);
                int employeeId = await _toDoService.GetEmployeeIdAsync(id);
                string strStatus = status ? "Opened" : "Closed";
                Message = $"ToDo status with id {id} was changed to '{strStatus}' successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("Index", "ToDo", new { id = employeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }        

        // GET: ToDoController/Duplicate
        public async Task<IActionResult> Duplicate(int id)
        {
            try
            {
                await _toDoService.DuplicateAsync(id);
                int employeeId = await _toDoService.GetEmployeeIdAsync(id);
                Message = $"ToDo with id {id} duplicated successfully.";
                TempData["SuccessMessage"] = Message;
                await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("Index", "ToDo", new { id = employeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }
        #endregion
    }
}
