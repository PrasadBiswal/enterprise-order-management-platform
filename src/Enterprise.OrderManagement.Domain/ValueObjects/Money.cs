namespace Enterprise.OrderManagement.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentNullException(nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    public bool IsZero() => Amount == 0;
}
