using Microsoft.EntityFrameworkCore;
using OrderSystem.Infrastructure;
using OrderSystem.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICES CONFIGURATION ---

// Add Controllers with JSON options to handle frontend communication
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Essential: Prevents 500 errors in Many-to-Many relationships (Orders <-> Products)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // Essential: Standardizes JSON properties to camelCase (lowercase first letter)
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection for Services
builder.Services.AddScoped<OrderService>();

// CORS Policy: Allows your React app (Vite) to talk to this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Your React URL
              .AllowAnyHeader()
              .AllowAnyMethod(); // Crucial: Allows POST, PUT (Edit), and DELETE
    });
});

var app = builder.Build();

// --- 2. MIDDLEWARE PIPELINE ---

// Enable Swagger in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Security and Routing (Order is critical here!)
app.UseCors("ReactPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();