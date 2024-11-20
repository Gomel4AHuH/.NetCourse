using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ToDoApp.Interfaces;
using ToDoApp.Models;
using ToDoApp.Dtos.Employee.Authorization;
using ToDoApp.Mappers;
using ToDoApp.Dtos.Employee;
using System.Configuration;
using NuGet.Configuration;
using ToDoAppAPI.Dtos.Token;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoApp.Controllers
{
    public class EmployeeController(IEmployeeService service, IConfiguration configuration) : Controller
    {
        private readonly IEmployeeService _employeeService = service;
        private readonly IConfiguration _configuration = configuration;
        private string? Message;

        #region Authorization
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
                
        public async Task<IActionResult> Login(LoginDto loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            HttpResponseMessage response = await _employeeService.LoginAsync(loginModel);
            
            var test = response.Content.ReadAsStringAsync();
            
            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            TokenDto tokenDto = new(arrResult["accessToken"].ToString(), arrResult["refreshToken"].ToString());

            SetTokensInsideCookie(tokenDto, HttpContext);

            if (response.IsSuccessStatusCode)
            {                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View(loginModel);
            }
        }
        private void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", tokenDto.AccessToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:expiryInHours"])),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

            context.Response.Cookies.Append("refreshToken", tokenDto.RefreshToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }

        public async Task<IActionResult> Logout()
        {            
            HttpResponseMessage response = await _employeeService.LogoutAsync();

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
                
        public async Task<IActionResult> Register(RegisterDto registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            HttpResponseMessage response = await _employeeService.RegisterAsync(registerModel);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View(registerModel);
            }
        }
        #endregion

        #region Indexes
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
                return View();
            }           
        }
        #endregion

        #region Actions
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee details not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("Index");
            }

            string[] specialities = _configuration.GetSection("Specialities").Get<string[]>();
            //SelectList selectList = new(specialities);
            //ViewData["Specialities"] = selectList;
            
            ViewData["Specialities"] = specialities;
            ViewData["Email"] = employee.Email;
            ViewData["UserName"] = employee.UserName;

            return View(employee.ToEditEmployeeDto());
        }
                
        public async Task<IActionResult> Edit(Guid id, EditEmployeeDto editEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(editEmployeeDto);
            }

            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee details not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await _employeeService.UpdateAsync(editEmployeeDto, employee);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Employee with id {id} updated successfully.";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee details not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee deleting not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("Index");
            }

            return View(employee);            
        }
                
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            HttpResponseMessage response = await _employeeService.DeleteAsync(id);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Employee with id {id} and all todos deleted successfully.";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserName(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee changing username not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("ChangeUserName", "Employee", new { id });
            }

            return View(employee.ToChangeUserNameDto());
        }

        public async Task<IActionResult> ChangeUserName(ChangeUserNameDto changeUserNameDto)
        {
            if (!ModelState.IsValid)
            {
                return View(changeUserNameDto);
            }

            HttpResponseMessage response = await _employeeService.ChangeUserNameAsync(changeUserNameDto);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Username for user '{changeUserNameDto.UserName}' changed to '{changeUserNameDto.NewUserName}' successfully";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Edit", "Employee", new { id = changeUserNameDto.Id });
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View(changeUserNameDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeEmail(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee changing email not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("ChangeEmail", "Employee", new { id });
            }

            return View(employee.ToChangeEmailDto());
        }

        public async Task<IActionResult> ChangeEmail(ChangeEmailDto changeEmailDto)
        {
            if (!ModelState.IsValid)
            {
                return View(changeEmailDto);
            }

            HttpResponseMessage response = await _employeeService.ChangeEmailAsync(changeEmailDto);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Email for user '{changeEmailDto.Email}' changed to '{changeEmailDto.NewEmail}' successfully";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Edit", "Employee", new { id = changeEmailDto.Id });
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View(changeEmailDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(Guid id)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee changing password not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("ChangePassword", "Employee", new { id });
            }

            return View(employee.ToChangePasswordDto());
        }

        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordDto);
            }

            HttpResponseMessage response = await _employeeService.ChangePasswordAsync(changePasswordDto);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Password for user '{changePasswordDto.Email}' changed successfully";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Edit", "Employee", new { id = changePasswordDto.Id });
            }
            else
            {
                ModelState.AddModelError("", arrResult["detail"].ToString());
                return View(changePasswordDto);
            }
        }
        #endregion
    }
}
