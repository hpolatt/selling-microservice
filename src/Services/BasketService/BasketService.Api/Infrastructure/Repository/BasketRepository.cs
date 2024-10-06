using System;
using System.Text.Json;
using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Domain.Models;
using StackExchange.Redis;

namespace BasketService.Api.Infrastructure.Repository;

public class BasketRepository : IBasketRepository
{
    private readonly ILogger logger;
    private readonly IConnectionMultiplexer redis;
    private readonly IDatabase database;

    public BasketRepository(ILoggerFactory loggerFactory, IConnectionMultiplexer redis)
    {
        this.logger = loggerFactory.CreateLogger<BasketRepository>();
        this.redis = redis;
        database = redis.GetDatabase();
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
        return await database.KeyDeleteAsync(basketId);
    }

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var data = await database.StringGetAsync(basketId);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
    }

    public IEnumerable<string> GetUsers()
    {
        var server = GetServer();
        var data = server.Keys();

        return data?.Select(k => k.ToString());
    }

    private IServer GetServer()
    {
        var endpoint = redis.GetEndPoints();
        return redis.GetServer(endpoint.First());
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        var created = await database.StringSetAsync(basket.BuyerId, JsonSerializer.Serialize(basket));

        if (!created)
        {
            logger.LogError("Basket could not be updated");
            return null;
        }

        logger.LogInformation("Basket updated successfully");

        return await GetBasketAsync(basket.BuyerId);
    }
}
