using Enterprise.OrderManagement.Application.Handlers;
using Enterprise.OrderManagement.Application.Interfaces;
using Enterprise.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Enterprise.OrderManagement.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------

// Controllers (required for attribute routing)
builder.Services.AddControllers();

// Swagger (dev-time only tooling)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core (InMemory for dev/test — will switch in prod)
builder.Services.AddDbContext<OrderManagementDbContext>(options =>
{
    options.UseInMemoryDatabase("OrderManagementDb");
});

// Dependency Injection
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CreateOrderHandler>();

var app = builder.Build();

// --------------------
// Middleware pipeline
// --------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseAuthorization();

// 🔴 REQUIRED: enables controllers
app.MapControllers();

app.Run();

// 🔴 REQUIRED FOR WebApplicationFactory (.NET 6+)
public partial class Program { }
