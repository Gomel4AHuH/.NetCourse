using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Dtos.ToDo
{
    public record CreateToDoDto()
    {        
        [Required]        
        public required string Name { get; set;}
        
        [Required]
        public required string Description { get; set; }
                
        public bool IsClosed { get; set;}

        [Required]        
        public Guid EmployeeId { get; set; }
    }
}
