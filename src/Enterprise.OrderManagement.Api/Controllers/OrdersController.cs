using Enterprise.OrderManagement.Application.Commands;
using Enterprise.OrderManagement.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.OrderManagement.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _handler;

    public OrdersController(CreateOrderHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var orderId = await _handler.HandleAsync(command);
        return CreatedAtAction(nameof(Create), new { id = orderId }, null);
    }
}
