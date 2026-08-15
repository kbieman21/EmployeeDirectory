using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Services;
using EmployeeDirectory.Domain.Interfaces;
using EmployeeDirectory.Infrastructure.Data;
using EmployeeDirectory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Api.Middleware;
using EmployeeDirectory.Application.Mappings;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);

var autoMapperLicenseKey =
    builder.Configuration["AutoMapper:LicenseKey"]
    ?? throw new InvalidOperationException(
        "The AutoMapper license key was not configured.");


// Add services to the container.


var connectionString =
    builder.Configuration.GetConnectionString("EmployeeDirectoryConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'EmployeeDirectoryConnection' was not found.");

builder.Services.AddDbContext<EmployeeDirectoryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(
    configuration =>
    {
        configuration.LicenseKey = autoMapperLicenseKey;
    },
    typeof(MappingProfile).Assembly);


builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PasswordHasher<object>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
