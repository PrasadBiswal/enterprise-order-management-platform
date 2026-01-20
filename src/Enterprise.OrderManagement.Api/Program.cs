using Enterprise.OrderManagement.Application.Handlers;
using Enterprise.OrderManagement.Application.Interfaces;
using Enterprise.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Add services
// --------------------

// Controllers (MANDATORY for controller-based APIs)
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core (InMemory for now)
builder.Services.AddDbContext<OrderManagementDbContext>(options =>
{
    options.UseInMemoryDatabase("OrderManagementDb");
});

// Dependency Injection
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CreateOrderHandler>();

var app = builder.Build();

// --------------------
// Configure middleware
// --------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 🔴 THIS IS WHAT YOU WERE MISSING
app.MapControllers();

app.Run();
