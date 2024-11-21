
namespace ToDoAppAPI.Models
{
    public class ToDo
    {        
        public string Id { get; set; } = Guid.NewGuid().ToString();                
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsClosed { get; set; }
        public string EmployeeId { get; set; }    }
}
