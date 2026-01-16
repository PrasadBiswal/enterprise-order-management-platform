namespace Enterprise.OrderManagement.Application.Commands;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyCollection<CreateOrderItem> Items
);

public sealed record CreateOrderItem(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string Currency
);
