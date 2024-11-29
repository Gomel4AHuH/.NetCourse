using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Dtos.Token;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Mappers;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class EmployeeController(IEmployeeRepository employeeRepository, ITokenService tokenService) : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ITokenService _tokenService = tokenService;
        private const int fileMaxSize = 2097152;

        [HttpPost("register")]        
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (registerDto.EmployeePhoto?.Length >= fileMaxSize)
            {
                ModelState.AddModelError("File", "The file is too large.");
            }

            string result = await _employeeRepository.RegisterAsync(registerDto);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Registration problem", result);
            }

            TokenDto tokenDto = await _tokenService.CreateTokenAsync(registerDto.Email, true);

            if (tokenDto is null) 
            {
                Log.Error(result);
                throw new ProblemException("Token creation error", result);
            }           

            _tokenService.SetTokensInsideCookie(tokenDto, HttpContext);

            result = $"Employee with email {registerDto.Email} registered successfully";

            Log.Information(result);

            return Ok(tokenDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _employeeRepository.ValidateUserAsync(loginDto);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Login problem", result);
            }

            TokenDto tokenDto = await _tokenService.CreateTokenAsync(loginDto.Email, true);

            _tokenService.SetTokensInsideCookie(tokenDto, HttpContext);

            result = $"Employee with email {loginDto.Email} logged in successfully";

            Log.Information(result);

            return Ok(tokenDto);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _tokenService.DeleteTokensInsideCookie(HttpContext);

            string result = $"Employee with email 'test' logged out successfully";

            Log.Information(result);

            return Ok();
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _employeeRepository.ChangePasswordAsync(changePasswordDto);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Change password problem", result);
            }

            result = $"Password for employee '{changePasswordDto.Email}' changed successfully";

            Log.Information(result);

            return Ok();
        }

        [HttpPost("changeemail")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDto changeEmailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            string result = await _employeeRepository.ChangeEmailAsync(changeEmailDto);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Change email problem", result);
            }

            result = $"Email for employee '{changeEmailDto.Email}' changed to '{changeEmailDto.NewEmail}' successfully";

            Log.Information(result);

            return Ok();
        }

        [HttpPost("changeusername")]
        [Authorize]
        public async Task<IActionResult> ChangeUserName(ChangeUserNameDto changeUserNameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _employeeRepository.ChangeUserNameAsync(changeUserNameDto);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Change username problem", result);
            }

            result = $"Username for employee '{changeUserNameDto.UserName}' changed to '{changeUserNameDto.NewUserName}' successfully";

            Log.Information(result);

            return Ok();
        }        

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                        
            var employees = await _employeeRepository.GetAllAsync();

            if (employees is not null)
            {
                var employeeDto = employees.Select(s => s.ToEmployeeDto());
                               
                return Ok(employeeDto);
            }

            Log.Information("No employees found");
            return Ok(new List<Employee>());
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeRepository.GetByIdAsync(id);

            string result = string.Empty;

            if (employee is null)
            {
                result = $"Employee with id '{id}' not found";
                Log.Error(result);
                throw new ProblemException("Get employee by id problem", result);
            }

            result = $"Employee with id '{id}' returned successfully";
            Log.Information(result);

            return Ok(employee.ToEmployeeDto());
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (updateEmployeeDto.EmployeePhoto?.Length >= fileMaxSize)
            {
                ModelState.AddModelError("File", "The file is too large.");
            }

            string result = await _employeeRepository.UpdateAsync(id, updateEmployeeDto);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Update employee problem", result);
            }            

            result = $"Employee with id '{id}' updated successfully";

            Log.Information(result);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _employeeRepository.DeleteAsync(id);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Delete employee problem", result);
            }

            result = $"Employee with id '{id}' and all todos deleted successfully";
            Log.Information(result);

            return Ok();                 
        }        
    }
}