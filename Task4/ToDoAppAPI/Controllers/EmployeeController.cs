using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        [HttpPost("register")]        
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (registerDto.EmployeePhoto?.Length >= 2097152)
            {
                ModelState.AddModelError("File", "The file is too large.");
            }

            string result = await _employeeRepository.RegisterAsync(registerDto);

            if (result != "")
            {
                Log.Error(result);
                throw new ProblemException("Registration problem", result);
            }

            TokenDto tokenDto = await _tokenService.CreateToken(registerDto.Email, true);

            if (tokenDto == null) 
            {
                Log.Error(result);
                throw new ProblemException("Token creation error", result);
            }           

            _tokenService.SetTokensInsideCookie(tokenDto, HttpContext);

            result = $"User with email {registerDto.Email} registered successfully";

            Log.Information(result);
            //return Ok(result);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _employeeRepository.ValidateUser(loginDto);

            if (result != "")
            {
                Log.Error(result);
                throw new ProblemException("Login problem", result);
            }

            TokenDto tokenDto = await _tokenService.CreateToken(loginDto.Email, true);

            _tokenService.SetTokensInsideCookie(tokenDto, HttpContext);

            result = $"User with email {loginDto.Email} logged in successfully";

            Log.Information(result);

            //return Ok(result);
            return Ok(tokenDto);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _tokenService.DeleteTokensInsideCookie(HttpContext);

            string result = $"User with email 'test' logged out successfully";

            Log.Information(result);

            //return Ok(result);
            return Ok();
        }

        [HttpPost("changepassword")]
        //[Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _employeeRepository.ChangePasswordAsync(changePasswordDto);

            if (result != "")
            {
                Log.Error(result);
                throw new ProblemException("Change password problem", result);
            }

            result = $"Password for user '{changePasswordDto.Email}' changed successfully";

            Log.Information(result);

            //return Ok(result);
            return Ok();
        }

        [HttpPost("changeemail")]
        //[Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDto changeEmailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //string token = await HttpContext.GetTokenAsync("accessToken");
            //var accessToken = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "accessToken");
            //string token1 = await HttpContext.GetTokenAsync("refreshToken");

            /*string result = "You are not authorized";

            if (string.IsNullOrEmpty(token))
            {                
                Log.Error("result");
                throw new ProblemException("Change email problem", result);
            }*/

            string result = await _employeeRepository.ChangeEmailAsync(changeEmailDto);

            if (result != "")
            {
                Log.Error(result);
                throw new ProblemException("Change email problem", result);
            }

            result = $"Email for user '{changeEmailDto.Email}' changed to '{changeEmailDto.NewEmail}' successfully";

            Log.Information(result);

            //return Ok(result);
            return Ok();
        }

        [HttpPost("changeusername")]
        //[Authorize]
        public async Task<IActionResult> ChangeUserName(ChangeUserNameDto changeUserNameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _employeeRepository.ChangeUserNameAsync(changeUserNameDto);

            if (result != "")
            {
                Log.Error(result);
                throw new ProblemException("Change username problem", result);
            }

            result = $"Username for user '{changeUserNameDto.UserName}' changed to '{changeUserNameDto.NewUserName}' successfully";

            Log.Information(result);

            //return Ok(result);
            return Ok();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeRepository.ForgotPasswordAsync(forgotPasswordDto);

            if (result != "")
            {
                Log.Error(result);
                throw new ProblemException("Forgot password problem", result);
            }

            result = $"Password for user '{forgotPasswordDto.Email}' reseted successfully";

            Log.Information(result);

            //return Ok(result);
            return Ok();
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employees = await _employeeRepository.GetAllAsync();

            var employeeDto = employees.Select(s => s.ToEmployeeDto());

            return Ok(employeeDto);
        }
        
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee.ToEmployeeDto());
        }

        [HttpPut]
        [Route("{id}")]
        //[Authorize]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (updateEmployeeDto.EmployeePhoto?.Length >= 2097152)
            {
                ModelState.AddModelError("File", "The file is too large.");
            }                      

            //var employee = await _employeeRepository.UpdateAsync(id, updateEmployeeDto.ToEmployeeFromUpdate());
            var employee = await _employeeRepository.UpdateAsync(id, updateEmployeeDto);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            Log.Information($"User with email {employee.Email} updated successfully");

            return Ok(employee.ToEmployeeDto());
        }

        [HttpDelete]
        [Route("{id}")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeRepository.DeleteAsync(id);

            if (employee == null)
            {
                return NotFound("Employee does not exist");
            }

            Log.Information($"User with email {employee.Email} deleted successfully");

            return Ok(employee);                 
        }        
    }
}