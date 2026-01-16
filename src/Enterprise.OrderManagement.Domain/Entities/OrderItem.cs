using Enterprise.OrderManagement.Domain.ValueObjects;

namespace Enterprise.OrderManagement.Domain.Entities;

public sealed class OrderItem
{
    public Guid ProductId { get; }
    public int Quantity { get; }
    public Money UnitPrice { get; }

    public OrderItem(Guid productId, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (unitPrice.IsNegative())
            throw new ArgumentException("Unit price cannot be negative.");

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Money TotalPrice =>
        new(UnitPrice.Amount * Quantity, UnitPrice.Currency);
}
