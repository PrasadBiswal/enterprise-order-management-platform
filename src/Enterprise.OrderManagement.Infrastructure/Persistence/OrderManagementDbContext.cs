using Enterprise.OrderManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.OrderManagement.Infrastructure.Persistence;

public sealed class OrderManagementDbContext : DbContext
{
    public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
}
