using System.ComponentModel.DataAnnotations;

namespace ToDoAppAPI.Dtos.Employee
{
    public record LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
