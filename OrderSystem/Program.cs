using Microsoft.EntityFrameworkCore;
using OrderSystem.Infrastructure;
using OrderSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES CONFIGURATION
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Isso garante que o C# aceite nomes vindos do JS (camelCase)
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Dependency Injection
builder.Services.AddScoped<OrderService>();

// CORS Policy Configuration
builder.Services.AddCors(options => {
    options.AddPolicy("ReactPolicy", policy => {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build(); // <--- The "app" is created here

// 2. MIDDLEWARE PIPELINE (Order is critical here!)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: UseCors MUST come before UseAuthorization
app.UseCors("ReactPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();