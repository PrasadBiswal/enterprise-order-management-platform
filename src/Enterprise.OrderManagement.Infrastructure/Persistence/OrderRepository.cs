using Microsoft.EntityFrameworkCore;
using Enterprise.OrderManagement.Application.Interfaces;
using Enterprise.OrderManagement.Domain.Entities;

namespace Enterprise.OrderManagement.Infrastructure.Persistence;

public sealed class OrderRepository : IOrderRepository
{
	private readonly OrderManagementDbContext _dbContext;
	public OrderRepository (OrderManagementDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public async Task AddAsync( Order order, CancellationToken cancellationToken = default)
	{
		await _dbContext.Orders.AddAsync(order, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}