using System.ComponentModel.DataAnnotations;

namespace ToDoAppAPI.Dtos.Employee
{
    public record ChangeEmailDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewEmail { get; set; }
    }
}
