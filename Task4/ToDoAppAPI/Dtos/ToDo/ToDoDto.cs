
namespace ToDoAppAPI.Dtos.ToDo
{
    public record ToDoDto
    {
        
        public string Id { get; set; }
                
        public string Name { get; set; }
                
        public required string Description { get; set; }
                
        public bool IsClosed { get; set; }
                
        public string EmployeeId { get; set; }
    }
}
