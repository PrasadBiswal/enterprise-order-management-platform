using Enterprise.OrderManagement.Domain.Entities;

namespace Enterprise.OrderManagement.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
}
