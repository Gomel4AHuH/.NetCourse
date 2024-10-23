using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoAppAPI.Models
{
    public class ToDo(Guid id, string name, string description, bool isClosed, Guid employeeId)
    {
        [Key]
        public Guid Id { get; set; } = id;

        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Name")]
        public required string Name { get; set; } = name;

        [Required]
        [Column(TypeName = "varchar(100)")]
        [DisplayName("Description")]
        public required string Description { get; set; } = description;

        [Column(TypeName = "bit")]
        [DisplayName("IsClosed")]
        public bool IsClosed { get; set; } = isClosed;

        [Required]
        [DisplayName("EmployeeId")]
        public Guid EmployeeId { get; set; } = employeeId;
    }
}
