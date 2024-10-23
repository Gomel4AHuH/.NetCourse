using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Mappers;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class EmployeeController(UserManager<Employee> userManager, ITokenService tokenService, SignInManager<Employee> signInManager, IWebHostEnvironment environment, IEmployeeRepository employeeRepository) : ControllerBase
    {
        private readonly UserManager<Employee> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly SignInManager<Employee> _signInManager = signInManager;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) 
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Employee employee = new()
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    MiddleName = registerDto.MiddleName,
                    Birthday = registerDto.Birthday,
                    Speciality = registerDto.Speciality,
                    EmploymentDate = registerDto.EmploymentDate,
                    EmployeePhotoPath = CreatePhoto(registerDto)
                };

                IdentityResult createdEmployee = await _userManager.CreateAsync(employee, registerDto.Password);

                if (createdEmployee.Succeeded)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(employee, "Employee");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewEmployeeDto
                            {
                                UserName = employee.UserName,
                                Email = employee.Email,
                                Token = _tokenService.CreateToken(employee),
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                MiddleName = employee.MiddleName,
                                Birthday = employee.Birthday,
                                Speciality = employee.Speciality,
                                EmploymentDate = employee.EmploymentDate,
                                EmployeePhotoPath = employee.EmployeePhotoPath
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);                        
                    }
                }
                else
                {
                    return StatusCode(500, createdEmployee.Errors);
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Employee employee = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (employee == null) return Unauthorized("Invalid username!");

            var result = await _signInManager.CheckPasswordSignInAsync(employee, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            return Ok(
                new NewEmployeeDto
                {
                    UserName = employee.UserName,
                    Email = employee.Email,
                    Token = _tokenService.CreateToken(employee)                    
                }
            );
        }

        [HttpGet]        
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var employees = await _employeeRepository.GetAllAsync();

            var employeeDto = employees.Select(s => s.ToEmployeeDto());

            return Ok(employeeDto);
        }
        
        [HttpGet("{id}")]
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
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeRepository.UpdateAsync(id, updateEmployeeDto.ToEmployeeFromUpdate(id));

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee.ToEmployeeDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeRepository.DeleteAsync(id);

            if (employee == null)
            {
                return NotFound("Employee does not exist");
            }

            return Ok(employee);
        }

        private string CreatePhoto(RegisterDto registerDto)
        {
            if (registerDto.EmployeePhoto != null)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                fileName += Path.GetExtension(registerDto.EmployeePhoto.FileName);

                string fullPath = _environment.WebRootPath + "/photos/" + fileName;
                using (var stream = System.IO.File.Create(fullPath))
                {
                    registerDto.EmployeePhoto.CopyTo(stream);
                }

                return fileName;
            }
            else
            {
                return "";
            }

        }
    }
}