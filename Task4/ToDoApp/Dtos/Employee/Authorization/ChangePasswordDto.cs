namespace ToDoApp.Dtos.Employee.Authorization
{
    public record ChangePasswordDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}
