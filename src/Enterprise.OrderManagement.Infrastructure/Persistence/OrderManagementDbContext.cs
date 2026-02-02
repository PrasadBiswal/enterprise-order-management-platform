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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(order =>
        {
            order.HasKey(o => o.Id);

            order.OwnsMany(o => o.Items, item =>
            {
                item.WithOwner().HasForeignKey("OrderId");
                item.HasKey(i => i.Id);

                item.OwnsOne(i => i.UnitPrice, money =>
                {
                    money.Property(m => m.Amount);
                    money.Property(m => m.Currency);
                });
            });
        });
    }
}
