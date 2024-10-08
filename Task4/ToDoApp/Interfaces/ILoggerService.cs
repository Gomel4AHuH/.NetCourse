﻿using ToDoApp.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.Interfaces

{
    public interface ILoggerService
    {
        Task<List<Logger>> GetAllAsync(string sortOrder, string searchString, int? pageNumber);
        Task CreateAsync(string message);
    }
}