using System.ComponentModel.DataAnnotations;

namespace ToDoAppAPI.Dtos.Employee
{
    public record ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; }        
    }
}
