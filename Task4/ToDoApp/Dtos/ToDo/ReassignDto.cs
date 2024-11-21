
namespace ToDoApp.Dtos.ToDo
{
    public record ReassignDto
    {
        public Guid ToDoId { get; set; }
        public string NewEmployeeId { get; set; }
    }
}
