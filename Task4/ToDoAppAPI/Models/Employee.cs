using Microsoft.AspNetCore.Identity;

namespace ToDoAppAPI.Models
{
    public class Employee : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Speciality { get; set; }

        public DateOnly? EmploymentDate { get; set; }

        public string? EmployeePhotoPath { get; set; }
    }
}
