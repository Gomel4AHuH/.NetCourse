using System.ComponentModel.DataAnnotations;

namespace ToDoAppAPI.Dtos.Employee
{
    public record RegisterDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public DateOnly? Birthday { get; set; }
        [Required]
        public string? Speciality { get; set; }
        [Required]
        public DateOnly? EmploymentDate { get; set; }
        
        [Display(Name = "File")]
        public IFormFile? EmployeePhoto { get; set; }
    }
}
