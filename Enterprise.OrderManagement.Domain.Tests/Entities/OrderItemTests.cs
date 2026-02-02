using Enterprise.OrderManagement.Domain.Entities;
using Enterprise.OrderManagement.Domain.ValueObjects;
using Xunit;

namespace Enterprise.OrderManagement.Domain.Tests.Entities;

public class OrderItemTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateOrderItem()
    {
        var productId = Guid.NewGuid();
        var money = new Money(100, "USD");

        var item = new OrderItem(productId, 2, money);

        Assert.Equal(productId, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(200, item.TotalPrice.Amount);
        Assert.Equal("USD", item.TotalPrice.Currency);
    }

    [Fact]
    public void Create_WithZeroQuantity_ShouldThrow()
    {
        var productId = Guid.NewGuid();
        var money = new Money(100, "USD");

        Assert.Throws<ArgumentException>(() =>
            new OrderItem(productId, 0, money));
    }
}
