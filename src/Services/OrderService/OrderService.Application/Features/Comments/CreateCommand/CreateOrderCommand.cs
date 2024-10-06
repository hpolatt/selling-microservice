using System;
using MediatR;
using OrderService.Domain.Models;

namespace OrderService.Application.Features.Comments.CreateCommand;

public class CreateOrderCommand : IRequest<bool>
{
    private readonly List<OrderItemDto> orderItems;

    public string UserName { get; private set; }

    public string City { get; private set; }

    public string Street { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }
    public string CardNumber { get; private set; }
    public string CarHolderName { get; private set; }
    public string CardSecurityNumber { get; private set; }
    public DateTime CardExpiration { get; private set; }
    public int CardTypeId { get; private set; }

    public IEnumerable<OrderItemDto> OrderItems => orderItems;

    public string CorrelationId { get; set; }

    public CreateOrderCommand()
    {
        orderItems = new List<OrderItemDto>();
    }
    
    public CreateOrderCommand(List<BasketItem> basketItems, string userId, string userName, string city, string street, string state, string country, string zipcode, 
            string cardNumber, string cardHolderName, DateTime cardExpiration, string cardSecurityNumber, int cardTypeId): this()
    {
        orderItems = basketItems.Select(x => new OrderItemDto()
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            PictureUrl = x.PictureUrl,
            UnitPrice = x.UnitPrice,
            Units = x.Quantity
        }).ToList();

        UserName = userName;
        City = city;
        Street = street;
        State = state;
        Country = country;
        ZipCode = zipcode;
        CardNumber = cardNumber;
        CarHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber; 
        CardTypeId = cardTypeId;
    }
     
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        public string PictureUrl { get; set; }
    }
    

}
