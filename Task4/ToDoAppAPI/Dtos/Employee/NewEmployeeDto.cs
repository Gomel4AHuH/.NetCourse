
namespace ToDoAppAPI.Dtos.Employee
{
    public record NewEmployeeDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Speciality { get; set; }
        public DateOnly? EmploymentDate { get; set; }
        public byte[]? EmployeePhoto { get; set; }
    }
}
