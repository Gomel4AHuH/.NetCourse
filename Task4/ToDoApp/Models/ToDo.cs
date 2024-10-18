using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models
{
    public class ToDo()
    {
        [Key]
        public Guid Id { get; set;}
        
        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Name")]
        public required string Name { get; set;}
        
        [Required]
        [Column(TypeName = "varchar(100)")]
        [DisplayName("Description")]
        public required string Description { get; set; }
        
        [Column(TypeName = "bit")]
        [DisplayName("IsClosed")]
        public bool IsClosed { get; set;}

        [Required]
        [DisplayName("EmployeeId")]
        public Guid EmployeeId { get; set; }
    }
}
