using System;
using MediatR;
using OrderService.Domain.AggregateModels.BuyerAggregate;


namespace OrderService.Domain.Events;

public class BuyerAndPaymentMethodVerifiedDomainEvent: INotification
{
    public Buyer Buyer { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public Guid OrderId { get; private set; }

    public BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod paymentMethod, Guid orderId)
    {
        Buyer = buyer;
        PaymentMethod = paymentMethod;
        OrderId = orderId;
    }

}
