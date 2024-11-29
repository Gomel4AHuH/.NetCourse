using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ToDoAppAPI.Dtos.ToDo;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Mappers;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class ToDoController(IToDoRepository toDoRepository) : ControllerBase
    {
        private readonly IToDoRepository _toDoRepository = toDoRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<ToDo> toDos = await _toDoRepository.GetAllAsync();

            if (toDos is not null)
            {
                return Ok(toDos);
            }

            Log.Information("No todos found");
            return Ok(new List<ToDo>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ToDo toDo = await _toDoRepository.GetByIdAsync(id);

            if (toDo is null)
            {
                string result = "ToDo does not exist.";
                Log.Error(result);
                throw new ProblemException("Get by id problem", result);
            }

            return Ok(toDo);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoDto createToDoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Employee employee = await _toDoRepository.GetEmployeeByIdAsync(createToDoDto.EmployeeId);

            string result = string.Empty;

            if (employee is null)
            {
                result = $"Employee with id {createToDoDto.EmployeeId} does not exist for creating ToDo.";
                Log.Error(result);
                throw new ProblemException("Create problem", result);
            }

            ToDo toDo = createToDoDto.ToToDoFromCreate(createToDoDto.EmployeeId);

            await _toDoRepository.CreateAsync(toDo);            

            result = $"ToDo with id {toDo.Id} created successfully.";
            Log.Information(result);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateToDoDto updateToDoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _toDoRepository.UpdateAsync(id, updateToDoDto.ToToDoFromUpdate(id));
            
            if (!string.IsNullOrEmpty(result))
            {                
                Log.Error(result);
                throw new ProblemException("Update problem", result);
            }

            result = $"ToDo with id {id} updated successfully.";
            Log.Information(result);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _toDoRepository.DeleteAsync(id);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Delete problem", result);
            }            

            result = $"ToDo with id {id} deleted successfully.";
            Log.Information(result);

            return Ok();
        }

        [HttpGet("ByEmployeeId/{id}")]
        public async Task<IActionResult> GetAllByEmployeeId(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<ToDo> toDos = await _toDoRepository.GetAllByEmployeeIdAsync(id);

            if (toDos is not null)
            {
                return Ok(toDos);
            }

            Log.Information("No todos found");
            return Ok(new List<ToDo>());            
        }

        [HttpGet]
        [Route("Duplicate/{id}")]
        public async Task<IActionResult> Duplicate([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _toDoRepository.DuplicateAsync(id);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Duplicate problem", result);
            }            

            result = $"ToDo with id {id} duplicated successfully.";
            Log.Information(result);

            return Ok();
        }

        [HttpPut]
        [Route("StatusChange/{id}")]
        public async Task<IActionResult> StatusChange([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _toDoRepository.StatusChangeAsync(id);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Status change problem", result);
            }            

            result = $"ToDo status with id {id} changed successfully.";
            Log.Information(result);

            return Ok();
        }

        [HttpPut]
        [Route("Reassign")]
        public async Task<IActionResult> Reassign([FromBody] ReassignDto reassignDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _toDoRepository.ReassignAsync(reassignDto);

            if (!string.IsNullOrEmpty(result))
            {
                Log.Error(result);
                throw new ProblemException("Reassign problem", result);
            }

            result = $"ToDo with id {reassignDto.ToDoId} reassigned to employee with id {reassignDto.NewEmployeeId} successfully";

            Log.Information(result);

            return Ok();
        }
    }
}