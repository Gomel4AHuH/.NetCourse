using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models
{
    public class Employee
    {
        public Employee()
        {
            
        }
        public Employee(string firstName, string lastName, string middleName, DateOnly birthday, string speciality, DateOnly employmentDate)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Birthday = birthday;
            Speciality = speciality;
            EmploymentDate = employmentDate;
        }

        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

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
        public required string MiddleName { get; set; }
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

        [NotMapped]
        [DisplayName("Avatar image")]
        public IFormFile AvatarImage { get; set; }
        //public List<ToDo> ToDos { get; set; }
    }
}
