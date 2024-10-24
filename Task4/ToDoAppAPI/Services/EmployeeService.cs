using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Services
{
    public class EmployeeService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Employee> _signinManager;
        public EmployeeService(UserManager<Employee> userManager, ITokenService tokenService, SignInManager<Employee> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }

        public async Task<NewEmployeeDto> Register([FromBody] RegisterDto registerDto)
        {
            Employee employee = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            await _userManager.CreateAsync(employee, registerDto.Password);

            await _userManager.AddToRoleAsync(employee, "User");

            NewEmployeeDto newEmployeeDto = new NewEmployeeDto
            {
                UserName = employee.UserName,
                Email = employee.Email
            };

            return newEmployeeDto;
             /*   new NewEmployeeDto
                {
                    UserName = employee.UserName,
                    Email = employee.Email,
                    Token = _tokenService.CreateToken(employee)
                }   */
        }
    }
}
