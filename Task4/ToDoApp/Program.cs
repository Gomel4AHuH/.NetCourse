using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Areas.Identity.Data;
using ToDoApp.Data;
using ToDoApp.Interfaces;
using ToDoApp.Services;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ToDoAppAuthContextConnection") ?? throw new InvalidOperationException("Connection string 'ToDoAppAuthContextConnection' not found.");

builder.Services.AddDbContext<ToDoAppAuthContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ToDoAppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ToDoAppUser>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<ILoggerService, LoggerService>();

builder.Services.AddDefaultIdentity<ToDoAppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ToDoAppAuthContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
