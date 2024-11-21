
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class ToDo()
    {
        public Guid Id { get; set;}        
        public required string Name { get; set;}
        [Required]
        public string Description { get; set; }
        public bool IsClosed { get; set;}
        public Guid EmployeeId { get; set; }
    }
}
