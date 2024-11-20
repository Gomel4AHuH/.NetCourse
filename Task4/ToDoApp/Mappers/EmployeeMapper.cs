using ToDoApp.Dtos.Employee;
using ToDoApp.Dtos.Employee.Authorization;
using ToDoApp.Models;

namespace ToDoApp.Mappers
{
    public static class EmployeeMapper
    {
        public static EditEmployeeDto ToEditEmployeeDto(this Employee employee)
        {            
            return new EditEmployeeDto
            {
                Id = employee.Id,                
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                Birthday = employee.Birthday,
                Speciality = employee.Speciality,
                EmploymentDate = employee.EmploymentDate,
                EmployeePhotoStr = employee.EmployeePhotoStr
            };
        }

        public static ChangeEmailDto ToChangeEmailDto(this Employee employee)
        {
            return new ChangeEmailDto
            {
                Id = employee.Id,
                Email = employee.Email,
                NewEmail = ""
            };
        }

        public static ChangeUserNameDto ToChangeUserNameDto(this Employee employee)
        {
            return new ChangeUserNameDto
            {
                Id = employee.Id,
                UserName = employee.UserName,
                NewUserName = ""
            };
        }

        public static ChangePasswordDto ToChangePasswordDto(this Employee employee)
        {
            return new ChangePasswordDto
            {
                Id = employee.Id,
                Email = employee.Email,
                CurrentPassword = "",
                NewPassword = "",
                NewPasswordConfirmation = ""
            };
        }
    }
}
