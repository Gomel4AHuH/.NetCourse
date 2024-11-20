using System.ComponentModel.DataAnnotations;

namespace ToDoAppAPI.Dtos.Employee
{
    public record ChangeUserNameDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string NewUserName { get; set; }
    }
}
