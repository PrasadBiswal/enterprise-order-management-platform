using Enterprise.OrderManagement.Domain.Enums;
using Enterprise.OrderManagement.Domain.ValueObjects;

namespace Enterprise.OrderManagement.Domain.Entities;

public sealed class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; }
    public Guid CustomerId { get; }
    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private Order()
    {

    }

    public Order(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId is required.");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.Draft;
    }

    public void AddItem(OrderItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Cannot modify order once submitted.");

        _items.Add(item);
    }
    public Money CalculateTotal(string currency)
    {
        if (!_items.Any())
            throw new InvalidOperationException("Order must contain at least one item.");

        var total = _items.Sum(i => i.TotalPrice.Amount);
        return new Money(total, currency);
    }

    public void Submit()
    {
        if (!_items.Any())
            throw new InvalidOperationException("Cannot submit an empty order.");

        Status = OrderStatus.Submitted;
    }
}
