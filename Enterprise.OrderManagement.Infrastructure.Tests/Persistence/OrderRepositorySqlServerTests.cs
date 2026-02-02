using Enterprise.OrderManagement.Domain.Entities;
using Enterprise.OrderManagement.Domain.ValueObjects;
using Enterprise.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Enterprise.OrderManagement.Infrastructure.Tests.Persistence;

public class OrderRepositorySqlServerTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistOrder_UsingSqlServer()
    {
        // 1️⃣ Create UNIQUE database name
        var databaseName = $"OrderManagement_Test_{Guid.NewGuid()}";

        // 2️⃣ SQL Server LocalDB connection string
        var connectionString =
            $"Server=(localdb)\\mssqllocaldb;Database={databaseName};Trusted_Connection=True;";

        // 3️⃣ Configure DbContext to use SQL Server
        var options = new DbContextOptionsBuilder<OrderManagementDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        // 4️⃣ Create database schema
        using (var context = new OrderManagementDbContext(options))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        Guid orderId;

        // 5️⃣ Persist Order
        using (var context = new OrderManagementDbContext(options))
        {
            var repository = new OrderRepository(context);

            var order = new Order(Guid.NewGuid());

            order.AddItem(
                new OrderItem(
                    Guid.NewGuid(),
                    2,
                    new Money(100, "USD")));

            await repository.AddAsync(order, CancellationToken.None);

            orderId = order.Id;
        }

        // 6️⃣ Reload and Assert
        using (var context = new OrderManagementDbContext(options))
        {
            var savedOrder = await context.Orders
                .Include(o => o.Items)
                .SingleAsync(o => o.Id == orderId);

            Assert.Single(savedOrder.Items);

            var item = savedOrder.Items.First();

            Assert.Equal(2, item.Quantity);
            Assert.Equal(100, item.UnitPrice.Amount);
            Assert.Equal("USD", item.UnitPrice.Currency);
        }

        // 7️⃣ Cleanup database
        using (var context = new OrderManagementDbContext(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
    }
}
