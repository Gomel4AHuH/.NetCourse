using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models
{
    public class ToDo()
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set;}
        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Name")]
        public required string Name { get; set;}
        [Required]
        [Column(TypeName = "varchar(100)")]
        [DisplayName("Description")]
        public required string Description { get; set; }

        [Column(TypeName = "boolean")]
        [DisplayName("IsClosed")]
        public bool IsClosed { get; set;}

        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("EmployeeId")]
        public int EmployeeId { get; set; }
    }
}
