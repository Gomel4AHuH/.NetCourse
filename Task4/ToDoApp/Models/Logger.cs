using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ToDoApp.Models
{
    public class Logger(string message)
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        [DisplayName("Message")]
        public string Message { get; set; } = message;

        [Required]
        [Column(TypeName = "datetime")]
        [DisplayName("ActionDateTime")]
        public DateTime ActionDateTime { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "varchar(200)")]
        [DisplayName("Author")]
        public string Author { get; set; } = "email";
    }
}
