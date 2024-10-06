using System;
using MediatR;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Domain.Events;

public class OrderStartedDomainEvent: INotification
{
    public string UserName { get; }
    public int CartTypeId { get; }
    public string CardNumber { get; }
    public string CardHolderName { get; }
    public DateTime CardExpiration { get; }
    public string CardSecurityNumber { get; }
    public Order Order { get; }

    public OrderStartedDomainEvent(Order order, string userName, int cardTypeId, string cardNumber, string cardHolderName, DateTime cardExpiration, string cardSecurityNumber)
    {
        Order = order;
        UserName = userName;
        CartTypeId = cardTypeId;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber;
    }
}
