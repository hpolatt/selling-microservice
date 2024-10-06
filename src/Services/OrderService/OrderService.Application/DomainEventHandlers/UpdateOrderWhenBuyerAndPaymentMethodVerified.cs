using System;
using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Events;

namespace OrderService.Application.DomainEventHandlers;

public class UpdateOrderWhenBuyerAndPaymentMethodVerified : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
{
      private readonly IOrderRepository orderRepository;

    public UpdateOrderWhenBuyerAndPaymentMethodVerified(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
    }
    public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent buyerAndPaymentMethodEvent, CancellationToken cancellationToken)
    {
        var orderUpdate = await orderRepository.GetByIdAsync(buyerAndPaymentMethodEvent.OrderId);
        orderUpdate.SetBuyerId(buyerAndPaymentMethodEvent.Buyer.Id);
        orderUpdate.SetPaymentMethodId(buyerAndPaymentMethodEvent.PaymentMethod.Id);
    }
}
