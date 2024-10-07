using System;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Models.Basket;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Services;

public class BasketService : IBasketService
{
    private readonly IHttpClientFactory httpClient;

    public BasketService(IHttpClientFactory httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<BasketData> GetBasketAsync(string userId)
    {
        var client = httpClient.CreateClient("basket");
        return await client.GetResponseAsync<BasketData>(userId) ?? new BasketData(userId);
    }

    public async Task<BasketData> UpdateBasketAsync(BasketData currentBasket)
    {
        var client = httpClient.CreateClient("basket");
        return await client.PostGetResponseAsync<BasketData, BasketData>("basket", currentBasket);
    }
}
