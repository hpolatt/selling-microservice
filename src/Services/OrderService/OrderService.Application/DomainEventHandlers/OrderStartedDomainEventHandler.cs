using System;
using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.Events;

namespace OrderService.Application.DomainEventHandlers;

public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
{
    private readonly IBuyerRepository buyerRepository;

    public OrderStartedDomainEventHandler(IBuyerRepository buyerRepository)
    {
        this.buyerRepository = buyerRepository;
    }
    public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = orderStartedEvent.CartTypeId != 0 ? orderStartedEvent.CartTypeId : 1;
        var buyer = await buyerRepository.GetSingleAsync(x => x.Name == orderStartedEvent.UserName, x => x.PaymentMethods);
        
        bool buyerOriginallyExisted = buyer is not null;

        if (!buyerOriginallyExisted)
        {
            buyer = new Buyer(orderStartedEvent.UserName);
        }
        buyer.VerifyOrAddPaymentMethod(cardTypeId, $"Payment Method on {DateTime.UtcNow}",  orderStartedEvent.CardNumber, orderStartedEvent.CardSecurityNumber, orderStartedEvent.CardHolderName, orderStartedEvent.CardExpiration, orderStartedEvent.Order.Id);

        var buyerUpdated = buyerOriginallyExisted ? buyerRepository.Update(buyer) : await buyerRepository.AddAsync(buyer);

        await buyerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

    }
}
