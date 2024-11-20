using System.ComponentModel.DataAnnotations;

namespace ToDoAppAPI.Dtos.Employee
{
    public record ChangePasswordDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string NewPasswordConfirmation { get; set; }
    }
}
