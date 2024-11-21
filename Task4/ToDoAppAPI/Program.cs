using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using ToDoAppAPI.Data;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Models;
using ToDoAppAPI.Repositories;
using ToDoAppAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("ToDoAppAPIContextConnection") ?? throw new InvalidOperationException("Connection string 'ToDoAppAPIContextConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddProblemDetails(options => 
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        context.ProblemDetails.Extensions.TryAdd("traceId", activity.Id);
    };
});

builder.Services.AddExceptionHandler<ProblemExceptionHandler>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(config));


builder.Services.AddIdentity<Employee, IdentityRole>(options => 
{
    options.Password.RequireDigit = Convert.ToBoolean(config["Identity:PasswordRequireDigit"]);
    options.Password.RequireUppercase = Convert.ToBoolean(config["Identity:PasswordRequireUppercase"]);
    options.Password.RequireLowercase = Convert.ToBoolean(config["Identity:PasswordRequireLowercase"]);
    options.Password.RequireNonAlphanumeric = Convert.ToBoolean(config["Identity:PasswordRequireNonAlphanumeric"]);
    options.Password.RequiredLength = Convert.ToInt32(config["Identity:PasswordRequireLength"]);
    options.User.RequireUniqueEmail = Convert.ToBoolean(config["Identity:UserRequireUniqueEmail"]);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme =
    options.DefaultSignOutScheme =
    options.DefaultSignInScheme = 
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JWTSettings:Issuer"],
        ValidAudience = config["JWTSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:SigningKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
            if (!string.IsNullOrEmpty(accessToken))
                context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthentication();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();