using System;
using BasketService.Api.Core.Domain.Models;

namespace BasketService.Api.Core.Application.Repository;

public interface IBasketRepository
{
    Task<CustomerBasket> GetBasketAsync(string basketId);
    IEnumerable<string> GetUsers();
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task<bool> DeleteBasketAsync(string basketId);

}
