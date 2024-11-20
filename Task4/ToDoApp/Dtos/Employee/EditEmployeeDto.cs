namespace ToDoApp.Dtos.Employee
{
    public record EditEmployeeDto
    {
        public Guid Id { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Speciality { get; set; }
        public DateOnly EmploymentDate { get; set; }
        public IFormFile? EmployeePhotoImage { get; set; }
        public string? EmployeePhotoStr { get; set; }
        public byte[]? EmployeePhoto { get; set; }
    }
}
