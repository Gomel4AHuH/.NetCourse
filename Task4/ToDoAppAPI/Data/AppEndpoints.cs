using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Data
{
    public static class AppEndpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            #region Employees endpoints
            RouteGroupBuilder employees = app.MapGroup("/api/employees");

            //employees.MapGet("/register", async (ApplicationDbContext db) =>
            //    await db.Employees.ToListAsync());

            employees.MapGet("/", async (ApplicationDbContext db) =>
                await db.Employees.ToListAsync());

            employees.MapGet("/{id}", [Authorize] async (string id, ApplicationDbContext db) =>
            await db.Employees.FindAsync(id)
                    is Employee employee
                        ? Results.Ok(employee)
                        : Results.NotFound());

            employees.MapPost("/", [Authorize] async (Employee employee, ApplicationDbContext db) =>
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync();

                return Results.Created($"/employees/{employee.Id}", employee);
            });

            employees.MapPut("/{id}", [Authorize] async (string id, Employee inputEmployee, ApplicationDbContext db) =>
            {
                Employee employee = await db.Employees.FindAsync(id);

                if (employee is null) return Results.NotFound();

                employee.UserName = inputEmployee.UserName;
                employee.Email = employee.Email;
                //employee.Token = _tokenService.CreateToken(employee);
                employee.FirstName = employee.FirstName;
                employee.LastName = employee.LastName;
                employee.MiddleName = employee.MiddleName;
                employee.Birthday = employee.Birthday;
                employee.Speciality = employee.Speciality;
                employee.EmploymentDate = employee.EmploymentDate;
                employee.EmployeePhotoPath = employee.EmployeePhotoPath;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            employees.MapDelete("/{id}", [Authorize] async (string id, ApplicationDbContext db) =>
            {
                if (await db.Employees.FindAsync(id) is Employee employee)
                {
                    db.Employees.Remove(employee);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            });
            #endregion

            #region ToDos endpoints
            var todos = app.MapGroup("/api/todos");

            todos.MapGet("/", [Authorize] async (ApplicationDbContext db) =>
                await db.ToDos.ToListAsync());

            todos.MapGet("/{id}", [Authorize] async (Guid id, ApplicationDbContext db) =>
                await db.ToDos.FindAsync(id)
                    is ToDo todo
                        ? Results.Ok(todo)
                        : Results.NotFound());

            todos.MapPost("/", [Authorize] async (ToDo todo, ApplicationDbContext db) =>
            {
                db.ToDos.Add(todo);
                await db.SaveChangesAsync();

                return Results.Created($"/todos/{todo.Id}", todo);
            });

            todos.MapPut("/{id}", [Authorize] async (Guid id, ToDo inputTodo, ApplicationDbContext db) =>
            {
                var todo = await db.ToDos.FindAsync(id);

                if (todo is null) return Results.NotFound();

                todo.Name = inputTodo.Name;
                todo.Description = inputTodo.Description;
                todo.IsClosed = inputTodo.IsClosed;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            todos.MapDelete("/{id}", [Authorize] async (Guid id, ApplicationDbContext db) =>
            {
                if (await db.ToDos.FindAsync(id) is ToDo todo)
                {
                    db.ToDos.Remove(todo);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            });
            #endregion
        }
    }
}
