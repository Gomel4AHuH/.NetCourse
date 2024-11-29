using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ToDoApp.Dtos.ToDo;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController(IToDoService toDoService, IEmployeeService employeeService) : Controller
    {
        private readonly IToDoService _toDoService = toDoService;
        private readonly IEmployeeService _employeeService = employeeService;
        private string? Message;        

        #region Indexes
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
                ViewData["DescriptionSortParm"] = sortOrder == "description" ? "description_desc" : "description";
                ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";

                ViewData["CurrentFilter"] = searchString;
                IEnumerable<Employee> employees = await _employeeService.GetAllAsync();
                
                ViewData["Employees"] = employees;

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
                return View();
            }
        }

        public async Task<IActionResult> IndexByEmployee(string sortOrder, string searchString, int? pageNumber, Guid id)
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

                IEnumerable<Employee> employees = await _employeeService.GetAllAsync();
                ViewData["Employees"] = employees.OrderBy(e => e.LastName+e.FirstName+e.MiddleName+e.Birthday);

                List<ToDo> toDoList = await _toDoService.GetAllByEmployeeIdAsync(sortOrder, searchString, pageNumber, employee.Id);

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
                return View();
            }
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Create(Guid employeeId)
        {
            return View(new CreateToDoDto { Name = string.Empty, Description = string.Empty, EmployeeId = employeeId });
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateToDoDto createToDoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(createToDoDto);
                }

                HttpResponseMessage response = await _toDoService.CreateAsync(createToDoDto);

                string strResult = response.Content.ReadAsStringAsync().Result;

                var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

                if (response.IsSuccessStatusCode)
                {
                    Message = $"New ToDo with employee id {createToDoDto.EmployeeId} created successfully.";
                    TempData["SuccessMessage"] = Message;
                    return RedirectToAction("IndexByEmployee", "ToDo", new { id = createToDoDto.EmployeeId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                    return View(createToDoDto);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                ToDo toDo = await _toDoService.GetByIdAsync(id);
                if (toDo == null)
                {
                    Message = "ToDo details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    return RedirectToAction("IndexByEmployee");
                }

                return View(toDo);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ToDo toDo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(toDo);
                }

                ToDo ToDo = await _toDoService.GetByIdAsync(id);

                if (ToDo == null)
                {
                    Message = "Employee details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    return RedirectToAction("Index");
                }

                HttpResponseMessage response = await _toDoService.UpdateAsync(toDo);

                string strResult = response.Content.ReadAsStringAsync().Result;

                var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

                if (response.IsSuccessStatusCode)
                {
                    Message = $"ToDo with id {id} updated successfully.";
                    TempData["SuccessMessage"] = Message;
                    return RedirectToAction("IndexByEmployee", "ToDo", new { id = toDo.EmployeeId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                    return View();
                }                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
                
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                ToDo toDo = await _toDoService.GetByIdAsync(id);
                if (toDo == null)
                {
                    Message = "ToDo details not available with the Id : " + id;
                    TempData["ErrorMessage"] = Message;
                    return RedirectToAction("IndexByEmployee");
                }

                return View(toDo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            ToDo toDo = await _toDoService.GetByIdAsync(id);

            if (toDo == null)
            {
                Message = "ToDo deleting not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("IndexByEmployee");
            }

            return View(toDo);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection, string employeeId)
        {
            try
            {
                HttpResponseMessage response = await _toDoService.DeleteAsync(id);

                string strResult = response.Content.ReadAsStringAsync().Result;

                var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

                if (response.IsSuccessStatusCode)
                {                    
                    Message = $"Employee with id {id} and all todos deleted successfully.";
                    TempData["SuccessMessage"] = Message;
                    return RedirectToAction("IndexByEmployee", new { id = employeeId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> StatusChange(Guid id, bool status, string employeeId)
        {
            try
            {
                HttpResponseMessage response = await _toDoService.StatusChangeAsync(id);

                string strResult = response.Content.ReadAsStringAsync().Result;

                var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

                if (response.IsSuccessStatusCode)
                {
                    string strStatus = status ? "Opened" : "Closed";
                    Message = $"ToDo status with id {id} was changed to '{strStatus}' successfully.";
                    TempData["SuccessMessage"] = Message;
                    return RedirectToAction("IndexByEmployee", new { id = employeeId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Duplicate(Guid id, string employeeId)
        {
            try
            {
                HttpResponseMessage response = await _toDoService.DuplicateAsync(id);

                string strResult = response.Content.ReadAsStringAsync().Result;

                var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

                if (response.IsSuccessStatusCode)
                {
                    Message = $"ToDo with id {id} duplicated successfully.";
                    TempData["SuccessMessage"] = Message;
                    return RedirectToAction("IndexByEmployee", new { id = employeeId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reassign(Guid id, string newEmployeeId, string currentEmployeeId)
        {
            try
            {
                ReassignDto reassignDto = new()
                {
                    ToDoId = id,
                    NewEmployeeId = newEmployeeId
                };

                HttpResponseMessage response = await _toDoService.ReassignAsync(reassignDto);

                string strResult = response.Content.ReadAsStringAsync().Result;

                var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

                if (response.IsSuccessStatusCode)
                {
                    Message = $"ToDo with id {id} reassigned successfully.";
                    TempData["SuccessMessage"] = Message;
                    return RedirectToAction("IndexByEmployee", new { id = currentEmployeeId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                    return View();
                }                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        #endregion
    }
}
