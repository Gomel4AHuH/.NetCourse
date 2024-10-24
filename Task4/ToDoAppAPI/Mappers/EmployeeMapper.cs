using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToEmployeeDto(this Employee employee)
        {            
            return new EmployeeDto
            {
                Id = employee.Id,
                UserName = employee.UserName,
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Birthday = employee.Birthday,
                Speciality = employee.Speciality,
                EmploymentDate = employee.EmploymentDate,
                EmployeePhotoPath = employee.EmployeePhotoPath        
            };            
        }              

        public static Employee ToEmployeeFromUpdate(this UpdateEmployeeDto updateEmployeeDto)
        {
            return new Employee
            {
                UserName = updateEmployeeDto.UserName,
                Email = updateEmployeeDto.Email,
                FirstName = updateEmployeeDto.FirstName,
                LastName = updateEmployeeDto.LastName,
                MiddleName = updateEmployeeDto.MiddleName,
                Birthday = updateEmployeeDto.Birthday,
                Speciality = updateEmployeeDto.Speciality,
                EmploymentDate = updateEmployeeDto.EmploymentDate,
                EmployeePhotoPath = updateEmployeeDto.EmployeePhotoPath
            };
        }

        public static NewEmployeeDto ToNewEmployeeDto(this Employee employee)
        {
            return new NewEmployeeDto
            {   
                UserName = employee.UserName,
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Birthday = employee.Birthday,
                Speciality = employee.Speciality,
                EmploymentDate = employee.EmploymentDate,
                EmployeePhotoPath = employee.EmployeePhotoPath
            };
        }
    }
}
