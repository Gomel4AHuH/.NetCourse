
namespace ToDoAppAPI.Dtos.ToDo
{
    public record CreateToDoDto
    {                
        public string Name { get; set; }
                
        public string Description { get; set; }

        public bool IsClosed { get; set; }

        public string EmployeeId { get; set; }

    }
}
