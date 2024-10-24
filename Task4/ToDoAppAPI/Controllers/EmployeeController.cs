using Microsoft.AspNetCore.Mvc;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Mappers;

namespace ToDoAppAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class EmployeeController(IEmployeeRepository employeeRepository) : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        [HttpPost("register")]        
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {            
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newEmployeeDto = await _employeeRepository.RegisterAsync(registerDto);

                if (newEmployeeDto != null)
                {
                    return Ok(newEmployeeDto);
                }

                return StatusCode(500, "Registration failed");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newEmployeeDto = await _employeeRepository.LoginAsync(loginDto);

                if (newEmployeeDto != null)
                {
                    return Ok(newEmployeeDto);
                }

                return Unauthorized("Invalid username, username not found and/or password incorrect");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }                       
        }

        [HttpGet]        
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employees = await _employeeRepository.GetAllAsync();

                var employeeDto = employees.Select(s => s.ToEmployeeDto());

                return Ok(employeeDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }            
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            try
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
            catch (Exception e)
            {
                return StatusCode(500, e);
            }            
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _employeeRepository.UpdateAsync(id, updateEmployeeDto.ToEmployeeFromUpdate());

                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                return Ok(employee.ToEmployeeDto());
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }            
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
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
            catch (Exception e)
            {
                return StatusCode(500, e);
            }            
        }        
    }
}