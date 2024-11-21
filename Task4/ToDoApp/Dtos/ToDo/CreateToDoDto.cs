
namespace ToDoApp.Dtos.ToDo
{
    public record CreateToDoDto()
    {        
        public required string Name { get; set;}
        public required string Description { get; set; }                
        public bool IsClosed { get; set;}
        public Guid EmployeeId { get; set; }
    }
}
