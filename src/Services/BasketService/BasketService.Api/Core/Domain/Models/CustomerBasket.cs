using System;

namespace BasketService.Api.Core.Domain.Models;

public class CustomerBasket
{
    public string BuyerId { get; set; }
    public List<BasketItem> Items { get; set; } = new();
    public CustomerBasket()
    {
    }

    public CustomerBasket(string customerId)
    {
        BuyerId = customerId;
    }

    public decimal Total()
    {
        return Items.Sum(x => x.Quantity * x.UnitPrice);
    }

}
