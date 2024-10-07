using System;
using Web.ApiGateway.Models.Basket;

namespace Web.ApiGateway.Services.Interfaces;

public interface IBasketService
{
    Task<BasketData> GetBasketAsync(string userId);
    Task<BasketData> UpdateBasketAsync(BasketData basket);
}
