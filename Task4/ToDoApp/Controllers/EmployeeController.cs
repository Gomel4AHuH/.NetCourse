using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Areas.Identity.Data;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    
    public class EmployeeController(IEmployeeService service, ILoggerService loggerService, IToDoService toDoService, UserManager<ToDoAppUser> userManager) : Controller
    {
        private readonly IEmployeeService _employeeService = service;
        private readonly IToDoService _toDoService = toDoService;
        private readonly ILoggerService _loggerService = loggerService;
        private readonly UserManager<ToDoAppUser> _userManager = userManager;
        private string? Message;

        private string GetUserMail()
        {
            string name = _userManager.GetUserName(User);
            return name;
        }

        #region API
        //[HttpGet("", Name = nameof(GetAll))]
        //[Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            return await _employeeService.GetAllAsync();
        }

        //[HttpGet("{id:int}", Name = nameof(GetEmployee))]
        //[Route("GetById")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            return employee;
        }
        
        //[HttpPost("{employeeVM}", Name = nameof(AddEmployee))]
        //[Route("Add")]
        public async Task<ActionResult<Employee>> AddEmployee(EmployeeVM employeeVM)
        {
            await _employeeService.CreateAsync(employeeVM);
            return NoContent();
        }
        /*
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateEmployee(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            await _employeeService.UpdateAsync(employee);

            return NoContent();
        }*/
        
        //Access to the path 'D:\Work\Education\.NetCourse\Task4\ToDoApp\wwwroot\photos\' is denied.
        //[HttpDelete("{id:int}", Name = nameof(DeleteEmployee))]
        //[Route("Delete")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteAsync(id);

            return NoContent();
        }
        #endregion

        #region Actions
        // GET: EmployeeController
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["LastNameSortParm"] = sortOrder == "lastName" ? "lastName_desc" : "lastName";
                ViewData["FirstNameSortParm"] = sortOrder == "firstName" ? "firstName_desc" : "firstName";
                ViewData["MiddleNameSortParm"] = sortOrder == "middleName" ? "middleName_desc" : "middleName";
                ViewData["BirthdaySortParm"] = sortOrder == "birthday" ? "birthday_desc" : "birthday";
                ViewData["SpecialitySortParm"] = sortOrder == "speciality" ? "speciality_desc" : "speciality";
                ViewData["EmploymentDateSortParm"] = sortOrder == "employmentDate" ? "employmentDate_desc" : "employmentDate";

                ViewData["CurrentFilter"] = searchString;

                List<Employee> employeeList = await _employeeService.GetAllAsync(sortOrder, searchString, pageNumber);

                if (employeeList.Count == 0)
                {
                    Message = "No employees available for now.";
                    TempData["InfoMessage"] = Message;
                }

                return View(employeeList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _loggerService.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }           
        }        

        // GET: EmployeeController/Create
        public IActionResult Create()
        {
            //return PartialView();
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeVM employeeVM)
        {            
            try
            {
                await _employeeService.CreateAsync(employeeVM);
                Message = $"Employee created successfully.";
                TempData["SuccessMessage"] = Message;
                await _loggerService.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _loggerService.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                Employee employee = await _employeeService.GetByIdAsync(id);
                if (employee == null)
                {
                    Message = "Employee details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    await _loggerService.CreateAsync(Message, GetUserMail().ToString());
                    return RedirectToAction("Index");
                }

                EmployeeVM employeeVM = _employeeService.EmployeeToEmployeeVM(employee);

                ViewData["EmployeePhotoPath"] = employee.EmployeePhotoPath;

                return View(employeeVM);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _loggerService.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EmployeeVM employeeVM)
        {
            try
            {
                Employee employee = await _employeeService.GetByIdAsync(id);
                await _employeeService.UpdateAsync(employeeVM, employee);
                Message = $"Employee with id {id} updated successfully.";
                TempData["SuccessMessage"] = Message;
                await _loggerService.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _loggerService.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: EmployeeController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                Employee employee = await _employeeService.GetByIdAsync(id);
                if (employee == null)
                {
                    Message = "Employee details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    await _loggerService.CreateAsync(Message, GetUserMail().ToString());
                    return RedirectToAction("Index");
                }

                return View(employee);
                //return PartialView("Details", employee);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _loggerService.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);
            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            try
            {
                string ids = await _toDoService.GetIdsByEmployeeIdAsync(id);
                await _employeeService.DeleteAsync(id);
                Message = $"Employee with id {id} and todos with ids: {ids} deleted successfully.";
                TempData["SuccessMessage"] = Message;
                await _loggerService.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _loggerService.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }

        // GET: EmployeeController/Create
        public ActionResult CreateToDo()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateToDo(ToDo toDo, Guid id)
        {
            try
            {
                await _employeeService.CreateToDoAsync(toDo, id);
                Message = $"ToDo for employee with id {id} created successfully.";
                TempData["SuccessMessage"] = Message;
                await _loggerService.CreateAsync(Message, GetUserMail().ToString());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _loggerService.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }
        #endregion
    }
}
