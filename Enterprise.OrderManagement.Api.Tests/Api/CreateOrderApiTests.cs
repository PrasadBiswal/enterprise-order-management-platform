using Enterprise.OrderManagement.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;


namespace Enterprise.OrderManagement.Api.Tests.Api;

public class CreateOrderApiTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CreateOrderApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostOrders_ShouldCreateOrder()
    {
        var request = new
        {
            customerId = Guid.NewGuid(),
            items = new[]
            {
                new
                {
                    productId = Guid.NewGuid(),
                    quantity = 2,
                    unitPrice = 100,
                    currency = "USD"
                }
            }
        };

        var response = await _client.PostAsJsonAsync(
            "/api/orders",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    [Fact]
    public async Task PostOrders_WithEmptyItems_ShouldReturnBadRequest()
    {
        var request = new
        {
            customerId = Guid.NewGuid(),
            items = Array.Empty<object>()
        };

        var response = await _client.PostAsJsonAsync(
            "/api/orders",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task PostOrders_WithInvalidQuantity_ShouldReturnBadRequest()
    {
        var request = new
        {
            customerId = Guid.NewGuid(),
            items = new[]
            {
            new
            {
                productId = Guid.NewGuid(),
                quantity = 0, // invalid
                unitPrice = 100,
                currency = "USD"
            }
        }
        };

        var response = await _client.PostAsJsonAsync(
            "/api/orders",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task PostOrders_WithNullBody_ShouldReturnUnsupportedMediaType()
    {
        var response = await _client.PostAsync(
            "/api/orders",
            content: null);

        Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
    }


}
