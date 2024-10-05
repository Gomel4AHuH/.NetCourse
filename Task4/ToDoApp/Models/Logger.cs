using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ToDoApp.Models
{
    public class Logger
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        [DisplayName("Message")]
        public required string Message { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("ActionDateTime")]
        public required DateTime ActionDateTime { get; set; }

        public void Add(string message)
        {
            Message = message;
            ActionDateTime = DateTime.Now;
        }
    }
}
