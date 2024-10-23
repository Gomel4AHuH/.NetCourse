using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public AuthorizationController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var client = _clientFactory.CreateClient();
            var requestContent = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_configuration["ToDoAppAPI:TokenEndpoint"], requestContent);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                Response.Cookies.Append("jwtToken", token, new CookieOptions { HttpOnly = true, Secure = true });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View(loginModel);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");

            return RedirectToAction("Login", "Employee");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var client = _clientFactory.CreateClient();
            var requestContent = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_configuration["ToDoAppAPI:RegisterEndpoint"], requestContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Employee");
            }
            else
            {
                ModelState.AddModelError("", "Registration failed");
                return View(loginModel);
            }
        }
    }
}
