using System;

namespace Web.ApiGateway.Models.Basket;

public class AddBasketItemRequest
{
    public int CatalogItemId { get; set; }
    public int Quantity { get; set; }
    public string BasketId { get; set; }

    public AddBasketItemRequest()
    {
        Quantity = 1;
    }
}
