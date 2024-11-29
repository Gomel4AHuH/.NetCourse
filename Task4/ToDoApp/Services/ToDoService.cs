using ToDoApp.Interfaces;
using ToDoApp.Data;
using ToDoApp.Models;
using Newtonsoft.Json;
using ToDoApp.Dtos.ToDo;

namespace ToDoApp.Services
{
    public class ToDoService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IToDoService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        #region Private methods
        private async Task<HttpResponseMessage> GetHttpResponseAsync(string apiUrl, string action, JsonContent? content)
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
                    response = await client.PostAsync(apiUrl, content);
                    break;
                case "put":
                    response = await client.PutAsync(apiUrl, content);
                    break;
                case "delete":
                    response = await client.DeleteAsync(apiUrl);
                    break;
                default:
                    break;
            };

            return response;
        }

        private async Task<List<ToDo>> PreparePaginatedListAsync(string sortOrder, string searchString, int? pageNumber, IEnumerable<ToDo> toDos)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
                toDos = toDos.Where(e => e.Name.ToLower().Contains(searchString.ToLower())
                                      || e.Description.ToLower().Contains(searchString.ToLower()));
            }

            toDos = sortOrder switch
            {
                "name" => toDos.OrderBy(e => e.Name),
                "name_desc" => toDos.OrderByDescending(e => e.Name),
                "description" => toDos.OrderBy(e => e.Description),
                "description_desc" => toDos.OrderByDescending(e => e.Description),
                "status" => toDos.OrderBy(e => e.IsClosed),
                "status_desc" => toDos.OrderByDescending(e => e.IsClosed),
                _ => toDos.OrderBy(e => e.Id),
            };

            int pageSize = Int32.Parse(_configuration.GetSection("PageSizes").GetSection("ToDo").Value);
            return await PaginatedList<ToDo>.CreateAsync(toDos, pageNumber ?? 1, pageSize);
        }
        #endregion

        #region Actions
        public async Task<List<ToDo>> GetAllByEmployeeIdAsync(string sortOrder, string searchString, int? pageNumber, Guid id)
        {
            HttpResponseMessage responseBody = await GetHttpResponseAsync(_configuration["ToDoAppAPI:TodosByEmployeeEndpoint"] + id.ToString(), "get", null);

            IEnumerable<ToDo> toDos = [];

            if (responseBody.IsSuccessStatusCode)
            {
                string response = await responseBody.Content.ReadAsStringAsync();
                toDos = JsonConvert.DeserializeObject<List<ToDo>>(response);
            }

            return await PreparePaginatedListAsync(sortOrder, searchString, pageNumber, toDos);
        }

        public async Task<List<ToDo>> GetAllAsync(string sortOrder, string searchString, int? pageNumber)
        {

            HttpResponseMessage responseBody = await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDosEndpoint"], "get", null);

            IEnumerable<ToDo> toDos = [];

            if (responseBody.IsSuccessStatusCode)
            {
                string response = await responseBody.Content.ReadAsStringAsync();
                toDos = JsonConvert.DeserializeObject<List<ToDo>>(response);
            }

            return await PreparePaginatedListAsync(sortOrder, searchString, pageNumber, toDos);
        }

        public async Task<ToDo> GetByIdAsync(Guid id)
        {
            HttpResponseMessage httpResponseMessage = await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDoEndpoint"] + id.ToString(), "get", null);
                        
            if (!httpResponseMessage.IsSuccessStatusCode) return null;

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<ToDo>(response);
        }

        public async Task<HttpResponseMessage> DeleteAsync(Guid id)
        {
            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDoEndpoint"] + id.ToString(), "delete", null);
        }

        public async Task<List<ToDo>> GetAllByEmployeeIdAsync(Guid id)
        {
            HttpResponseMessage responseBody = await GetHttpResponseAsync(_configuration["ToDoAppAPI:TodosByEmployeeEndpoint"] + id.ToString(), "get", null);

            List<ToDo> toDos = [];

            if (responseBody.IsSuccessStatusCode)
            {
                string response = await responseBody.Content.ReadAsStringAsync();
                toDos = JsonConvert.DeserializeObject<List<ToDo>>(response);
            }

            return toDos;
        }

        public async Task<HttpResponseMessage> CreateAsync(CreateToDoDto createToDo)
        {
            JsonContent content = JsonContent.Create(createToDo);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDoEndpoint"], "post", content);
        }

        public async Task<HttpResponseMessage> UpdateAsync(ToDo toDo)
        {
            JsonContent content = JsonContent.Create(toDo);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDoEndpoint"] + toDo.Id.ToString(), "put", content);
        }

        public async Task<HttpResponseMessage> StatusChangeAsync(Guid id)
        {
            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDoStatusChangeEndpoint"] + id.ToString(), "put", null);
        }

        public async Task<HttpResponseMessage> DuplicateAsync(Guid id)
        {
            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDoDuplicateEndpoint"] + id.ToString(), "get", null);
        }

        public async Task<HttpResponseMessage> ReassignAsync(ReassignDto reassignDto)
        {
            JsonContent content = JsonContent.Create(reassignDto);

            return await GetHttpResponseAsync(_configuration["ToDoAppAPI:ToDoReassignEndpoint"], "put", content);
        }
        #endregion
    }
}
