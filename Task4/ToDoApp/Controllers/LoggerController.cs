﻿using Microsoft.AspNetCore.Mvc;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class LoggerController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly ILoggerService _logger;
        private string Message = "";

        public LoggerController(ILoggerService loggerService, ILoggerService logger)
        {
            _loggerService = loggerService;
            _logger = logger;
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
                await _logger.CreateAsync(ex.Message);
                return View();
            }
        }
    }
}
