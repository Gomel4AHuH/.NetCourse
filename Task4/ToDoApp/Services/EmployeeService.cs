using ToDoApp.Interfaces;
using ToDoApp.Models;
using ToDoApp.Data;
using Newtonsoft.Json;
using ToDoApp.Dtos.Employee.Authorization;
using ToDoApp.Dtos.Employee;
using Microsoft.IdentityModel.Tokens;

namespace ToDoApp.Services
{
    public class EmployeeService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IToDoService toDoService) : IEmployeeService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IToDoService _toDoService = toDoService;

        #region Private methods
        private async Task<HttpResponseMessage> GetHttpResponseAsync(string apiUrl, string action, JsonContent? content, MultipartFormDataContent multipartFormDataContent)
        {
            string? httpClientName = _configuration["ToDoAppHTTPClient:Name"];

            HttpClient client = _httpClientFactory.CreateClient(httpClientName ?? string.Empty);
                        
            HttpResponseMessage response = new();
                        
            switch (action)
            {
                case "get":                    
                    response = await client.GetAsync(apiUrl);
                    break;
                case "post":
                    response = await client.PostAsync(apiUrl, (content != null) ? content : multipartFormDataContent);
                    break;
                case "put":
                    response = await client.PutAsync(apiUrl, (content != null) ? content : multipartFormDataContent);
                    break;
                case "delete":
                    response = await client.DeleteAsync(apiUrl);
                    break;
                default:
                    break;
            };
            
            return response;
        }        

        private static void GetImageFromByteArray(Employee employee)
        {
            employee.EmployeePhotoStr = string.Empty;

            if (!employee.EmployeePhoto.IsNullOrEmpty())
            {
                string imreBase64Data = Convert.ToBase64String(employee.EmployeePhoto);
                employee.EmployeePhotoStr = string.Format("data:image/png;base64,{0}", imreBase64Data);
            }
        }

        private static byte[] IFormFileToByteArray(IFormFile formFile)
        {
            byte[] data = [];            

            if (formFile is not null)
            {
                using var br = new BinaryReader(formFile.OpenReadStream());
                data = br.ReadBytes((int)formFile.OpenReadStream().Length);
            }

            return data;
        }        
        #endregion

        #region Actions
        public async Task<HttpResponseMessage> LoginAsync(LoginDto loginModel)
        {            
            JsonContent content = JsonContent.Create(loginModel);
            
            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:LoginEndpoint"], "post", content, null);
        }

        public async Task<HttpResponseMessage> LogoutAsync()
        {
            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:LogoutEndpoint"], "post", null, null);
        }

        public async Task<HttpResponseMessage> RegisterAsync(RegisterDto registerModel)
        {
            byte[] data = IFormFileToByteArray(registerModel.EmployeePhotoImage);

            ByteArrayContent bytes = new(data);            

            MultipartFormDataContent multipartFormContent = new()
            {
                { new StringContent(registerModel.UserName), "UserName" },
                { new StringContent(registerModel.Email), "Email" },
                { new StringContent(registerModel.Password), "Password" },
                { new StringContent(registerModel.FirstName), "FirstName" },
                { new StringContent(registerModel.LastName), "LastName" },
                { new StringContent(registerModel.MiddleName ?? string.Empty), "MiddleName" },
                { new StringContent(registerModel.Birthday.ToString()), "Birthday" },
                { new StringContent(registerModel.Speciality), "Speciality" },
                { new StringContent(registerModel.EmploymentDate.ToString()), "EmploymentDate" }
            };
            if (!data.IsNullOrEmpty()) multipartFormContent.Add(bytes, "EmployeePhoto", registerModel.EmployeePhotoImage.FileName);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:RegisterEndpoint"], "post", null, multipartFormContent);           
        }

        public async Task<List<Employee>> GetAllAsync(string sortOrder, string searchString, int? pageNumber)
        {
            HttpResponseMessage responseBody = await GetHttpResponseAsync(_configuration["ToDoAppAPI:EmployeesEndpoint"], "get", null, null);

            IEnumerable<Employee> employees = [];

            if (responseBody.IsSuccessStatusCode)
            {
                string response = await responseBody.Content.ReadAsStringAsync();
                employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            }

            foreach (Employee employee in employees)
            {
                GetImageFromByteArray(employee);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.LastName.ToLower().Contains(searchString.ToLower())
                                              || e.FirstName.ToLower().Contains(searchString.ToLower())
                                              || (!e.MiddleName.IsNullOrEmpty() && e.MiddleName.ToLower().Contains(searchString.ToLower()))
                                              || e.Birthday.ToString().Contains(searchString.ToLower())
                                              || e.Speciality.ToLower().Contains(searchString.ToLower())
                                              || e.EmploymentDate.ToString().Contains(searchString.ToLower()));
            }
            
            employees = sortOrder switch
            {
                "lastName" => employees.OrderBy(e => e.LastName),
                "lastName_desc" => employees.OrderByDescending(e => e.LastName),
                "firstName" => employees.OrderBy(e => e.FirstName),
                "firstName_desc" => employees.OrderByDescending(e => e.FirstName),
                "middleName" => employees.OrderBy(e => e.MiddleName),
                "middleName_desc" => employees.OrderByDescending(e => e.MiddleName),
                "birthday" => employees.OrderBy(e => e.Birthday),
                "birthday_desc" => employees.OrderByDescending(e => e.Birthday),
                "speciality" => employees.OrderBy(e => e.Speciality),
                "speciality_desc" => employees.OrderByDescending(e => e.Speciality),
                "employmentDate" => employees.OrderBy(e => e.EmploymentDate),
                "employmentDate_desc" => employees.OrderByDescending(e => e.EmploymentDate),
                _ => employees.OrderBy(e => e.Id),
            };

            if (!String.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }

            int pageSize = Int32.Parse(_configuration.GetSection("PageSizes").GetSection("Employee").Value);
            
            return await PaginatedList<Employee>.CreateAsync(employees, pageNumber ?? 1, pageSize);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            HttpResponseMessage responseBody = await GetHttpResponseAsync(_configuration["ToDoAppAPI:EmployeesEndpoint"], "get", null, null);

            IEnumerable<Employee> employees = [];

            if (responseBody.IsSuccessStatusCode)
            {
                string response = await responseBody.Content.ReadAsStringAsync();
                employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            }

            employees = employees.OrderBy(e => e.LastName);

            return employees;
        }        

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            HttpResponseMessage responseBody = await GetHttpResponseAsync(_configuration["ToDoAppAPI:EmployeeEndpoint"] + id.ToString(), "get", null, null);

            if (!responseBody.IsSuccessStatusCode) return null;

            string response = await responseBody.Content.ReadAsStringAsync();

            Employee employee = JsonConvert.DeserializeObject<Employee>(response);

            GetImageFromByteArray(employee);

            return employee;
        }

        public async Task<HttpResponseMessage> DeleteAsync(Guid id)
        {
            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:EmployeeEndpoint"] + id.ToString(), "delete", null, null);
        }

        public async Task<HttpResponseMessage> UpdateAsync(EditEmployeeDto editEmployeeDto, Employee employee)
        {
            byte[] data = IFormFileToByteArray(editEmployeeDto.EmployeePhotoImage);

            ByteArrayContent bytes = new(data);                      

            MultipartFormDataContent multipartFormContent = new()
            {
                { new StringContent(editEmployeeDto.Id.ToString()), "Id" },
                { new StringContent(editEmployeeDto.FirstName), "FirstName" },
                { new StringContent(editEmployeeDto.LastName), "LastName" },
                { new StringContent(editEmployeeDto.MiddleName ?? string.Empty), "MiddleName" },
                { new StringContent(editEmployeeDto.Birthday.ToString()), "Birthday" },
                { new StringContent(editEmployeeDto.Speciality), "Speciality" },
                { new StringContent(editEmployeeDto.EmploymentDate.ToString()), "EmploymentDate" }
            };
            if (!data.IsNullOrEmpty()) multipartFormContent.Add(bytes, "EmployeePhoto", editEmployeeDto.EmployeePhotoImage.FileName);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:EmployeeEndpoint"] + employee.Id, "put", null, multipartFormContent);
        }

        public async Task<HttpResponseMessage> ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            JsonContent content = JsonContent.Create(changeEmailDto);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ChangeEmailEndpoint"], "post", content, null);
        }

        public async Task<HttpResponseMessage> ChangeUserNameAsync(ChangeUserNameDto changeUserNameDto)
        {
            JsonContent content = JsonContent.Create(changeUserNameDto);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ChangeUserNameEndpoint"], "post", content, null);
        }

        public async Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            JsonContent content = JsonContent.Create(changePasswordDto);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ChangePasswordEndpoint"], "post", content, null);
        }
        #endregion
    }
}
