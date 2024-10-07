﻿using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoService : IToDoService
    {
        //private readonly ToDoDbContext _toDoContext;
        private readonly ToDoAppDbContext _context;

        public ToDoService(ToDoAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToDo>> GetAllAsync()
        {
            return await _context.ToDos.ToListAsync();
        }
        public async Task<List<ToDo>> GetAllAsync(string sortOrder, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            IQueryable<ToDo> toDos = from e in _context.ToDos
                                             select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                toDos = toDos.Where(e => e.Name.Contains(searchString)
                                              || e.Description.Contains(searchString));
            }

            toDos = sortOrder switch
            {
                "id" => toDos.OrderBy(e => e.Id),
                "name" => toDos.OrderBy(e => e.Name),
                "name_desc" => toDos.OrderByDescending(e => e.Name),
                "description" => toDos.OrderBy(e => e.Description),
                "description_desc" => toDos.OrderByDescending(e => e.Description),
                _ => toDos.OrderBy(e => e.Id),
            };

            int pageSize = 10;
            return await PaginatedList<ToDo>.CreateAsync(toDos.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<ToDo> GetByIdAsync(int id)
        {
            return await _context.ToDos.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            ToDo toDo = await _context.ToDos.FindAsync(id);
            if (toDo != null)
            {
                _context.ToDos.Remove(toDo);
                await _context.SaveChangesAsync();
            }
        }
        public async Task CreateAsync(ToDo toDo)
        {
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToDo toDo)
        {
            _context.Update(toDo);
            await _context.SaveChangesAsync();
        }
    }
}
