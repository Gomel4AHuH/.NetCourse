namespace ToDoApp.Dtos.Employee.Authorization
{
    public record ChangeUserNameDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NewUserName { get; set; }
    }
}
