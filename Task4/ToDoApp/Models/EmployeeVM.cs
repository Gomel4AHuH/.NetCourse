using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class EmployeeVM
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string? Password { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string LastName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateOnly Birthday { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string Speciality { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        public DateOnly EmploymentDate { get; set; }

        public IFormFile? EmployeePhoto { get; set; }

        public string? EmployeePhotoImage { get; set; }
    }
}
