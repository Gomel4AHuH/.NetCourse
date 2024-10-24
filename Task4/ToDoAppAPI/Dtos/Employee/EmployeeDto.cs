
namespace ToDoAppAPI.Dtos.Employee
{
    public record EmployeeDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Speciality { get; set; }
        public DateOnly? EmploymentDate { get; set; }
        public string? EmployeePhotoPath { get; set; }
    }
}
