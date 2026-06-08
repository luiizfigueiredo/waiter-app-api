using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetEnv;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using WaiterApp.Data;
using WaiterApp.Hubs;
using WaiterApp.Middleware;
using WaiterApp.Notifications;
using WaiterApp.Repositories;
using WaiterApp.Repositories.Interfaces;
using WaiterApp.Services;
using WaiterApp.Services.Interfaces;

const string CorsPolicy = "frontend";

// Load .env from current directory (solution root) or parent of content root (project dir)
Env.Load();
Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", ".env"));

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// SignalR real-time
builder.Services.AddSignalR();
builder.Services.AddScoped<IOrderNotifier, OrderNotifier>();

// CORS (SignalR with credentials requires explicit origins, no wildcard)
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

app.UseMiddleware<GlobalErrorMiddleware>();

var uploadsPath = Path.Combine(builder.Environment.WebRootPath ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot"), "uploads");
Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles();

app.UseCors(CorsPolicy);

app.MapControllers();
app.MapHub<OrderHub>("/hubs/orders");

app.Run();
