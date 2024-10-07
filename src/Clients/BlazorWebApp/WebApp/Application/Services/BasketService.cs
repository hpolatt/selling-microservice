using System;
using WebApp.Application.Services.DTOs;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models.ViewModels;
using WebApp.Extensions;

namespace WebApp.Application.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient apiClient;
    private readonly IIdentityService identityService;
    private readonly ILogger<BasketService> logger;

    public BasketService(HttpClient apiClient, IIdentityService identityService, ILogger<BasketService> logger)
    {
        this.apiClient = apiClient;
        this.identityService = identityService;
        this.logger = logger;
    }

    public async Task<Basket> GetBasketAsync()
    {
        string username = await identityService.GetUsername();
        var response = await apiClient.GetResponseAsync<Basket>($"basket/{username}"); 
        return response ?? new Basket() { BuyerId = username };
    }


    public async Task<Basket> UpdateBasketAsync(Basket basket)
    {
        return await apiClient.PostGetResponseAsync<Basket, Basket>("basket/update", basket);
    }
    public async Task<Basket> AddItemToBasketAsync(int productId)
    {
        string username = await identityService.GetUsername();
        var model = new
        {
            CatalogItemId = productId,
            Quantity = 1,
            BasketId = username
        };

        return await apiClient.PostGetResponseAsync<Basket, object>("basket/update", model);
    }

    public Task CheckoutAsync(BasketDTO basket)
    {
        return apiClient.PostAsync("basket/checkout", basket);
    }
}
