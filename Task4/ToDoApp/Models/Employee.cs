using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public required string FirstName { get; set; }       
        [Required]
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }                
        [Required]
        public DateOnly Birthday { get; set; }        
        [Required]
        public required string Speciality { get; set; }
        [Required]
        public DateOnly EmploymentDate { get; set; }
        public IFormFile? EmployeePhotoImage { get; set; }
        public string? EmployeePhotoStr { get; set; }
        public byte[]? EmployeePhoto { get; set; }
    }
}
