using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Dtos.ToDo;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Mappers
{
    public static class ToDoMapper
    {
        public static ToDoDto ToToDoDto(this ToDo toDo)
        {
            return new ToDoDto
            {
                Id = toDo.Id,
                Name = toDo.Name,
                Description = toDo.Description,
                IsClosed = toDo.IsClosed,
                EmployeeId = toDo.EmployeeId,
            };
        }

        public static ToDo ToToDoFromCreate(this CreateToDoDto createToDoDto, string id)
        {
            return new ToDo
            {
                Name = createToDoDto.Name,
                Description = createToDoDto.Description,
                IsClosed = createToDoDto.IsClosed,
                EmployeeId = id
            };
        }
        /*
        public static Employee ToEmployeeFromUpdate(this UpdateEmployeeDto updateEmployeeDto, string id)
        {
            return new Employee
            {
                UserName = updateEmployeeDto.UserName,
                Email = updateEmployeeDto.Email,
                FirstName = updateEmployeeDto.FirstName,
                LastName = updateEmployeeDto.LastName,
                MiddleName = updateEmployeeDto.MiddleName,
                Birthday = updateEmployeeDto.Birthday,
                Speciality = updateEmployeeDto.Speciality,
                EmploymentDate = updateEmployeeDto.EmploymentDate,
                EmployeePhotoPath = updateEmployeeDto.EmployeePhotoPath
            };
        }*/
    }
}
