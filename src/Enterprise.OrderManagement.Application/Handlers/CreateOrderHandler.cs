using Enterprise.OrderManagement.Application.Commands;
using Enterprise.OrderManagement.Application.Interfaces;
using Enterprise.OrderManagement.Domain.Entities;
using Enterprise.OrderManagement.Domain.ValueObjects;

namespace Enterprise.OrderManagement.Application.Handlers;

public sealed class CreateOrderHandler
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository
            ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<Guid> HandleAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Null guard
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        // 2. Command-shape validation
        if (command.Items is null || !command.Items.Any())
            throw new ArgumentException("Order must contain at least one item.");

        // 3. Domain creation
        var order = new Order(command.CustomerId);

        foreach (var item in command.Items)
        {
            var orderItem = new OrderItem(
                item.ProductId,
                item.Quantity,
                new Money(item.UnitPrice, item.Currency));

            order.AddItem(orderItem);
        }

        // 4. Persist
        await _orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}
