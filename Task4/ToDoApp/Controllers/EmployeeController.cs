using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ToDoApp.Interfaces;
using ToDoApp.Models;
using ToDoApp.Dtos.Employee.Authorization;
using ToDoApp.Mappers;
using ToDoApp.Dtos.Employee;
using ToDoAppAPI.Dtos.Token;

namespace ToDoApp.Controllers
{
    public class EmployeeController(IEmployeeService service, IConfiguration configuration) : Controller
    {
        private readonly IEmployeeService _employeeService = service;
        private readonly IConfiguration _configuration = configuration;
        private string? Message;
        private const int fileMaxSize = 2097152;

        #region Private methods
        private void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", tokenDto.AccessToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:expiryInMinutes"])),
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
        private void DeleteTokensInsideCookie(HttpContext context)
        {
            context.Response.Cookies.Delete("accessToken");

            context.Response.Cookies.Delete("refreshToken");
        }

        private void PasswordValidation(ChangePasswordDto changePasswordDto) 
        {
            if (changePasswordDto.CurrentPassword == changePasswordDto.NewPassword)
            {
                ModelState.AddModelError(nameof(changePasswordDto.NewPassword),
                             "New password can't be the same as current password.");
            }

            if (changePasswordDto.NewPassword != changePasswordDto.NewPasswordConfirmation)
            {
                ModelState.AddModelError(nameof(changePasswordDto.NewPasswordConfirmation),
                             "New password and new password confimation should be the same.");
            }
        }

        private void UserNameValidation(ChangeUserNameDto changeUserNameDto)
        {
            if (changeUserNameDto.UserName == changeUserNameDto.NewUserName)
            {
                ModelState.AddModelError(nameof(changeUserNameDto.NewUserName),
                             "New username can't be the same as current username.");
            }
        }

        private void EmailValidation(ChangeEmailDto changeEmailDto)
        {
            if (changeEmailDto.Email == changeEmailDto.NewEmail)
            {
                ModelState.AddModelError(nameof(changeEmailDto.NewEmail),
                             "New email can't be the same as current email.");
            }
        }

        private void EmployeeValidation(EditEmployeeDto editEmployeeDto)
        {
            if (editEmployeeDto.EmployeePhotoImage?.Length >= fileMaxSize)
            {
                ModelState.AddModelError(nameof(editEmployeeDto.EmployeePhotoImage),
                             "Photo image size is too large. Please use images less than 2 mb");
            }
        }
        #endregion

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

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (!response.IsSuccessStatusCode)
            {                
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                return View(loginModel);
            }            

            TokenDto tokenDto = new(arrResult["accessToken"].ToString(), arrResult["refreshToken"].ToString());

            SetTokensInsideCookie(tokenDto, HttpContext);

            return RedirectToAction("Index", "Home");
        }        

        public async Task<IActionResult> Logout()
        {            
            HttpResponseMessage response = await _employeeService.LogoutAsync();

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                DeleteTokensInsideCookie(HttpContext);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            string[] specialities = _configuration.GetSection("Specialities").Get<string[]>();

            ViewData["Specialities"] = specialities;

            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerModel)
        {
            string[] specialities = _configuration.GetSection("Specialities").Get<string[]>();

            ViewData["Specialities"] = specialities;

            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            HttpResponseMessage response = await _employeeService.RegisterAsync(registerModel);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (!response.IsSuccessStatusCode)            
            {
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                return View(registerModel);
            }            

            return RedirectToAction("Login", "Employee");
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
                        
            ViewData["Specialities"] = specialities;
            ViewData["Email"] = employee.Email;
            ViewData["UserName"] = employee.UserName;

            return View(employee.ToEditEmployeeDto());
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditEmployeeDto editEmployeeDto)
        {
            Employee employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                Message = "Employee details not available with the Id : " + id;
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("Index");
            }

            EmployeeValidation(editEmployeeDto);

            string[] specialities = _configuration.GetSection("Specialities").Get<string[]>();

            ViewData["Specialities"] = specialities;
            ViewData["Email"] = employee.Email;
            ViewData["UserName"] = employee.UserName;
            editEmployeeDto.EmployeePhotoStr = employee.EmployeePhotoStr;

            if (!ModelState.IsValid)
            {
                return View(editEmployeeDto);
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
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
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

        [ValidateAntiForgeryToken]
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
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserName(ChangeUserNameDto changeUserNameDto)
        {
            UserNameValidation(changeUserNameDto);

            if (!ModelState.IsValid)
            {
                return View(changeUserNameDto);
            }

            HttpResponseMessage response = await _employeeService.ChangeUserNameAsync(changeUserNameDto);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Username for employee '{changeUserNameDto.UserName}' changed to '{changeUserNameDto.NewUserName}' successfully";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Edit", "Employee", new { id = changeUserNameDto.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDto changeEmailDto)
        {
            EmailValidation(changeEmailDto);

            if (!ModelState.IsValid)
            {
                return View(changeEmailDto);
            }

            HttpResponseMessage response = await _employeeService.ChangeEmailAsync(changeEmailDto);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Email for employee '{changeEmailDto.Email}' changed to '{changeEmailDto.NewEmail}' successfully";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Edit", "Employee", new { id = changeEmailDto.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            PasswordValidation(changePasswordDto);
            
            if (!ModelState.IsValid)
            {
                return View(changePasswordDto);
            }

            HttpResponseMessage response = await _employeeService.ChangePasswordAsync(changePasswordDto);

            string strResult = response.Content.ReadAsStringAsync().Result;

            var arrResult = (JObject)JsonConvert.DeserializeObject(strResult);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Password for employee '{changePasswordDto.Email}' changed successfully";
                TempData["SuccessMessage"] = Message;
                return RedirectToAction("Edit", "Employee", new { id = changePasswordDto.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, arrResult["detail"].ToString());
                return View(changePasswordDto);
            }
        }
        #endregion
    }
}
