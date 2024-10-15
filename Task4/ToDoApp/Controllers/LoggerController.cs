﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Areas.Identity.Data;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class LoggerController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly ILoggerService _logger;
        private readonly UserManager<ToDoAppUser> _userManager;
        private string Message = "";

        public LoggerController(ILoggerService loggerService, ILoggerService logger, UserManager<ToDoAppUser> userManager)
        {
            _loggerService = loggerService;
            _logger = logger;
            _userManager = userManager;
        }

        private async Task<string> GetUserMail()
        {
            ToDoAppUser user = await _userManager.GetUserAsync(User);
            return user.Email;
        }

        // GET: LoggerController
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["MessageSortParm"] = sortOrder == "message" ? "message_desc" : "message";
                ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
                ViewData["AuthorSortParm"] = sortOrder == "author" ? "author_desc" : "author";

                ViewData["CurrentFilter"] = searchString;

                List<Logger> loggerList = await _loggerService.GetAllAsync(sortOrder, searchString, pageNumber);

                if (loggerList.Count == 0)
                {
                    Message = "No logs available for now.";
                    TempData["InfoMessage"] = Message;
                }

                return View(loggerList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await _logger.CreateAsync(ex.Message, GetUserMail().ToString());
                return View();
            }
        }
    }
}
