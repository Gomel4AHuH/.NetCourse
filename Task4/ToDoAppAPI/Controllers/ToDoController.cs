using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Dtos.ToDo;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Mappers;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class ToDoController(IToDoRepository toDoRepository) : ControllerBase
    {
        private readonly IToDoRepository _toDoRepository = toDoRepository;              

        [HttpGet]        
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var toDos = await _toDoRepository.GetAllAsync();

            //var employeeDto = toDos.Select(s => s.ToEmployeeDto());

            return Ok(toDos);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDo = await _toDoRepository.GetByIdAsync(id);

            if (toDo == null)
            {
                return NotFound();
            }

            //return Ok(toDo.ToEmployeeDto());
            return Ok(toDo);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoDto createToDoDto, string employeeId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _toDoRepository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
            {
                return BadRequest("Employee not found");
            }

            var toDo = createToDoDto.ToToDoFromCreate(employee.Id);
            toDo.EmployeeId = employeeId;
            //commentModel.AppUserId = appUser.Id;
            await _toDoRepository.CreateAsync(toDo);
            //return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
            return Ok(toDo);
        }
        /*
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _toDoRepository.UpdateAsync(id, updateEmployeeDto.ToEmployeeFromUpdate(id));

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee.ToEmployeeDto());
        }*/

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDo = await _toDoRepository.DeleteAsync(id);

            if (toDo == null)
            {
                return NotFound("ToDo does not exist");
            }

            return Ok(toDo);
        }
    }
}