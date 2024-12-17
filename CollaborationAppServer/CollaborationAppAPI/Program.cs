using Microsoft.EntityFrameworkCore;
using CollaborationAppAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=10.24.8.14;Database=AppDB;User Id=AppDB;Password=test12345;Encrypt=False;TrustServerCertificate=True;"));



builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
