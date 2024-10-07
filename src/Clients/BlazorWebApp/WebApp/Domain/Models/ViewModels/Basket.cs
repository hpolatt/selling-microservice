using System;

namespace WebApp.Domain.Models.ViewModels;

public class Basket
{
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();

    public string BuyerId { get; set; }

    public decimal Total()
    {
        return Math.Round(Items.Sum(x => x.Price*x.Quantity), 2);
    }

    public Basket()
    {
        
    }

}
