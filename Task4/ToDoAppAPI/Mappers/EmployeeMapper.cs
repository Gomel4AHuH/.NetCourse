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
                EmployeePhoto = employee.EmployeePhoto
            };            
        }

        public static Employee ToEmployeeDto(this RegisterDto registerDto)
        {
            return new Employee
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                MiddleName = registerDto.MiddleName,
                Birthday = registerDto.Birthday,
                Speciality = registerDto.Speciality,
                EmploymentDate = registerDto.EmploymentDate
            };
        }

        public static Employee ToEmployeeFromUpdate(this UpdateEmployeeDto updateEmployeeDto)
        {
            return new Employee
            {
                FirstName = updateEmployeeDto.FirstName,
                LastName = updateEmployeeDto.LastName,
                MiddleName = updateEmployeeDto.MiddleName,
                Birthday = updateEmployeeDto.Birthday,
                Speciality = updateEmployeeDto.Speciality,
                EmploymentDate = updateEmployeeDto.EmploymentDate                
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
                EmployeePhoto = employee.EmployeePhoto
            };
        }
    }
}
