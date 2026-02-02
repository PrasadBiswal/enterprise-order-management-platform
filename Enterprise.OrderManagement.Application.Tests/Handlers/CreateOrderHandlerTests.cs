using Enterprise.OrderManagement.Application.Handlers;
using Enterprise.OrderManagement.Application.Interfaces;
using Enterprise.OrderManagement.Application.Commands;
using Enterprise.OrderManagement.Domain.Entities;
using Enterprise.OrderManagement.Domain.ValueObjects;
using Moq;
using Xunit;

namespace Enterprise.OrderManagement.Application.Tests.Handlers;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldPersistOrder()
    {
        // Arrange
        var repositoryMock = new Mock<IOrderRepository>();

        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateOrderHandler(repositoryMock.Object);

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new[]
            {
                new CreateOrderItem(
                    Guid.NewGuid(),
                    2,
                    100,
                    "USD")
            });

        // Act
        await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task HandleAsync_WithNoItems_ShouldThrow()
    {
        // Arrange
        var repositoryMock = new Mock<IOrderRepository>();

        var handler = new CreateOrderHandler(repositoryMock.Object);

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            Array.Empty<CreateOrderItem>());

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.HandleAsync(command, CancellationToken.None));

        // Ensure repository was never called
        repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
    [Fact]
    public async Task HandleAsync_WithNullCommand_ShouldThrow()
    {
        var repositoryMock = new Mock<IOrderRepository>();
        var handler = new CreateOrderHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            handler.HandleAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_WhenRepositoryFails_ShouldPropagateException()
    {
        // Arrange
        var repositoryMock = new Mock<IOrderRepository>();

        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB failure"));

        var handler = new CreateOrderHandler(repositoryMock.Object);

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new[]
            {
            // IMPORTANT: valid domain data
            new CreateOrderItem(
                Guid.NewGuid(),
                1,
                100m,
                "USD")
            });

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.HandleAsync(command, CancellationToken.None));
    }

}
