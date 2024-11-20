
namespace ToDoApp.Dtos.Employee.Authorization
{
    public record ChangeEmailDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
    }
}
