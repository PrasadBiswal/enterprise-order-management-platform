using Enterprise.OrderManagement.Domain.Entities;
using Enterprise.OrderManagement.Domain.ValueObjects;
using Xunit;

namespace Enterprise.OrderManagement.Domain.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void CreateOrder_WithValidCustomer_ShouldSucceed()
    {
        var customerId = Guid.NewGuid();

        var order = new Order(customerId);

        Assert.Equal(customerId, order.CustomerId);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void AddItem_ShouldAddItemToOrder()
    {
        var order = new Order(Guid.NewGuid());
        var item = new OrderItem(
            Guid.NewGuid(),
            2,
            new Money(50, "USD"));
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        order.AddItem(item);

        Assert.Single(order.Items);
    }

    [Fact]
    public void AddNullItem_ShouldThrow()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() =>
            order.AddItem(null!));
    }
}
