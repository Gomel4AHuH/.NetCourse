using ToDoAppAPI.Dtos.ToDo;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Mappers
{
    public static class ToDoMapper
    {
        public static ToDo ToToDoFromCreate(this CreateToDoDto createToDoDto, string employeeId)
        {
            return new ToDo
            {
                Name = createToDoDto.Name,
                Description = createToDoDto.Description,
                IsClosed = createToDoDto.IsClosed,
                EmployeeId = employeeId
            };
        }
        
        public static ToDo ToToDoFromUpdate(this UpdateToDoDto updateToDoDto, string id)
        {
            return new ToDo
            {
                Name = updateToDoDto.Name,
                Description = updateToDoDto.Description,
                IsClosed = updateToDoDto.IsClosed                
            };
        }
    }
}
