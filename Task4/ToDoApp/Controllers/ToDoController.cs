using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Areas.Identity.Data;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    /*[Route("api/ToDo")]
    [ApiController]*/
    public class ToDoController(IToDoService toDoService, ILoggerService logger, UserManager<ToDoAppUser> userManager, IEmployeeService employeeService) : Controller
    {
        private readonly IToDoService _toDoService = toDoService;
        private readonly ILoggerService _logger = logger;
        private readonly UserManager<ToDoAppUser> _userManager = userManager;
        private readonly IEmployeeService _employeeService = employeeService;
        private string? Message;

        private string GetUserMail()
        {
            string name = _userManager.GetUserName(User);
            return name;
        }

        #region API
        /*
        [HttpGet]
        //[Route("GetById")]
        public async Task<ActionResult<ToDo>> GetToDo(Guid id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);
            
            if (toDo == null)
            {
                return NotFound("ToDo not found.");
            }

            return toDo;
        }
        
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAll()
        {
            return await _toDoService.GetAllAsync();
        }
        
        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<ToDo>> GetToDo(Guid id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);

            if (toDo == null)
            {
                return NotFound("ToDo not found.");
            }

            return toDo;
        }
        
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<ToDo>> AddToDo(ToDo toDo)
        {
            await _toDoService.CreateAsync(toDo);
            return NoContent();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateToDo(Guid id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);

            if (toDo == null)
            {
                return NotFound("ToDo not found.");
            }

            await _toDoService.UpdateAsync(toDo);

            return NoContent();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteToDo(Guid id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }

            await _toDoService.DeleteAsync(id);

            return NoContent();
        }*/
        #endregion

        #region Actions
        // GET: ToDoController
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
                ViewData["DescriptionSortParm"] = sortOrder == "description" ? "description_desc" : "description";
                ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";

                ViewData["CurrentFilter"] = searchString;
                
                List<ToDo> toDoList;

                toDoList = await _toDoService.GetAllAsync(sortOrder, searchString, pageNumber);

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
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        public async Task<IActionResult> IndexEmployee(string sortOrder, string searchString, int? pageNumber, Guid id)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
                ViewData["DescriptionSortParm"] = sortOrder == "description" ? "description_desc" : "description";
                ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";

                ViewData["CurrentFilter"] = searchString;

                Employee employee = await _employeeService.GetByIdAsync(id);
                ViewData["Employee"] = employee;

                List<ToDo> toDoList = await _toDoService.GetAllByEmployeeIdAsync(sortOrder, searchString, pageNumber, id);

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
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: ToDoController/Create
        public ActionResult Create(Guid employeeId)
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
                //await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("IndexEmployee", "ToDo", new { id = toDo.EmployeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: ToDoController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                ToDo toDo = await _toDoService.GetByIdAsync(id);
                if (toDo == null)
                {
                    Message = "ToDo details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    //await _logger.CreateAsync(Message, GetUserMail().ToString());
                    return RedirectToAction("IndexEmployee");
                }
                return View(toDo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // POST: ToDoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ToDo toDo)
        {
            try
            {
                await _toDoService.UpdateAsync(toDo);
                Message = $"ToDo with id {toDo.Id} updated successfully.";
                TempData["SuccessMessage"] = Message;
                //await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("IndexEmployee", "ToDo", new { id = toDo.EmployeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: ToDoController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                ToDo toDo = await _toDoService.GetByIdAsync(id);
                if (toDo == null)
                {
                    Message = "ToDo details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    //await _logger.CreateAsync(Message, GetUserMail().ToString());
                    return RedirectToAction("IndexEmployee");
                }
                return View(toDo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: ToDoController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);
            return View(toDo);
        }

        // POST: ToDoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            try
            {
                Guid employeeId = await _toDoService.GetEmployeeIdAsync(id);
                await _toDoService.DeleteAsync(id);                
                Message = $"ToDo with id {id} deleted successfully.";
                TempData["SuccessMessage"] = Message;
                //await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("IndexEmployee", "ToDo", new { id = employeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: ToDoController/Close
        public async Task<IActionResult> StatusChange(Guid id, bool status)
        {
            try
            {
                await _toDoService.StatusChangeAsync(id);
                Guid employeeId = await _toDoService.GetEmployeeIdAsync(id);
                string strStatus = status ? "Opened" : "Closed";
                Message = $"ToDo status with id {id} was changed to '{strStatus}' successfully.";
                TempData["SuccessMessage"] = Message;
                //await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("IndexEmployee", "ToDo", new { id = employeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }        

        // GET: ToDoController/Duplicate
        public async Task<IActionResult> Duplicate(Guid id)
        {
            try
            {
                await _toDoService.DuplicateAsync(id);
                Guid employeeId = await _toDoService.GetEmployeeIdAsync(id);
                Message = $"ToDo with id {id} duplicated successfully.";
                TempData["SuccessMessage"] = Message;
                //await _logger.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction("IndexEmployee", "ToDo", new { id = employeeId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }
        #endregion
    }
}
