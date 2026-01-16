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
        _orderRepository = orderRepository;
    }

    public async Task<Guid> HandleAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = new Order(command.CustomerId);

        foreach (var item in command.Items)
        {
            var orderItem = new OrderItem(
                item.ProductId,
                item.Quantity,
                new Money(item.UnitPrice, item.Currency));

            order.AddItem(orderItem);
        }

        order.Submit();

        await _orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}
