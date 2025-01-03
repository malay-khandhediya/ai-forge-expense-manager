using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext before building the app
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ExpenseTrackerDB;"));

// Add controllers to the services
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection(); // Ensure HTTPS redirection is enabled
app.UseHsts();               // Enable HTTP Strict Transport Security

app.MapControllers(); // Ensure this is called after adding controllers

app.Run();