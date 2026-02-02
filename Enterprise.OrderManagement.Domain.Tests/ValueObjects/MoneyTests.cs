using Enterprise.OrderManagement.Domain.ValueObjects;
using Xunit;

namespace Enterprise.OrderManagement.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_WithPositiveAmount_ShouldSucceed()
    {
        var money = new Money(100, "USD");

        Assert.Equal(100, money.Amount);
        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new Money(-5, "USD"));
    }
}
