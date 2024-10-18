using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class EmployeeVM
    {
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
    }
}
