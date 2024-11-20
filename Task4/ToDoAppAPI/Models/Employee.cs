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

        public byte[]? EmployeePhoto { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
