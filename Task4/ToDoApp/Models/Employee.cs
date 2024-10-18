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
        [Column(TypeName = "varchar(50)")]
        [DisplayName("First name")]
        public required string FirstName { get; set; }
       
        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Last name")]
        public required string LastName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Middle name")]
        public string? MiddleName { get; set; }
                
        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Birthday")]
        public DateOnly Birthday { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Speciality")]
        public required string Speciality { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Employment date")]
        public DateOnly EmploymentDate { get; set; }

        [DisplayName("Employee photo")]
        public string EmployeePhotoPath { get; set; }
    }
}
