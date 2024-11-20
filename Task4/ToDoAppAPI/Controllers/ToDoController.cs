using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
    //[Authorize]
    public class ToDoController(IToDoRepository toDoRepository) : ControllerBase
    {
        private readonly IToDoRepository _toDoRepository = toDoRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDos = await _toDoRepository.GetAllAsync();

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
                string error = "ToDo does not exist.";
                Log.Error(error);
                throw new ProblemException("ToDo getting error", error);
            }

            return Ok(toDo);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoDto createToDoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _toDoRepository.GetEmployeeByIdAsync(createToDoDto.EmployeeId);

            if (employee == null)
            {
                string error = $"Employee with id {createToDoDto.EmployeeId} does not exist for creating ToDo.";
                Log.Error(error);
                throw new ProblemException("Employee does not exist", error);
            }

            var toDo = createToDoDto.ToToDoFromCreate(createToDoDto.EmployeeId);

            toDo = await _toDoRepository.CreateAsync(toDo);

            if (toDo == null)
            {
                string error = $"ToDo {toDo} cannot be created.";
                Log.Error(error);
                throw new ProblemException("Employee does not exist", error);
            }

            Log.Information($"ToDo with id {toDo.Id} created successfully.");

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateToDoDto updateToDoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDo = await _toDoRepository.UpdateAsync(id, updateToDoDto.ToToDoFromUpdate(id));

            if (toDo == null)
            {
                string error = "ToDo not found for updating.";
                Log.Error(error);
                throw new ProblemException("ToDo does not exist", error);
            }

            Log.Information($"ToDo with id {toDo.Id} updated successfully.");

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDo = await _toDoRepository.DeleteAsync(id);

            if (toDo == null)
            {
                string error = "ToDo not found for deleting.";
                Log.Error(error);
                throw new ProblemException("ToDo does not exist", error);
            }

            Log.Information($"ToDo with id {toDo.Id} deleted successfully.");

            return Ok();
        }

        [HttpGet("ByEmployeeId/{id}")]
        public async Task<IActionResult> GetAllByEmployeeId(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDos = await _toDoRepository.GetAllByEmployeeIdAsync(id);

            return Ok(toDos);
        }

        [HttpGet]
        [Route("Duplicate/{id}")]
        public async Task<IActionResult> Duplicate([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDo = await _toDoRepository.DuplicateAsync(id);

            if (toDo == null)
            {
                string error = "ToDo not found for duplicating.";
                Log.Error(error);
                throw new ProblemException("ToDo does not exist", error);
            }

            Log.Information($"ToDo with id {toDo.Id} duplicated successfully.");

            return Ok();
        }

        [HttpPut]
        [Route("StatusChange/{id}")]
        public async Task<IActionResult> StatusChange([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toDo = await _toDoRepository.StatusChangeAsync(id);

            if (toDo == null)
            {
                string error = "ToDo not found for status changing.";
                Log.Error(error);
                throw new ProblemException("ToDo does not exist", error);
            }

            Log.Information($"ToDo status with id {toDo.Id} changed successfully.");

            return Ok();
        }

        [HttpPut]
        [Route("Reassign")]
        public async Task<IActionResult> Reassign([FromBody] ReassignDto reassignDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = await _toDoRepository.ReassignAsync(reassignDto);

            if (result != "")
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