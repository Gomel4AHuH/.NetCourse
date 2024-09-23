using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    //public class ToDo(string name, string description, int employeeId)
    public class ToDo()
    {
        [Key]
        public int Id { get; set;}
        public required string Name { get; set;}
        public required string Description { get; set; }
        public bool IsClosed { get; set;}
        public int EmployeeId { get; set; }
    }
}
