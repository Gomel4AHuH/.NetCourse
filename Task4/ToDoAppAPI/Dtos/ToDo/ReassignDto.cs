using System.ComponentModel.DataAnnotations;

namespace ToDoAppAPI.Dtos.ToDo
{
    public class ReassignDto
    {
        [Required]
        public string ToDoId { get; set; }
        [Required]
        public string NewEmployeeId { get; set; }
    }
}
