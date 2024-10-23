using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }        
    }
}
