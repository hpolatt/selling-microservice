using System;
using EventBus.Base.Abstraction;
using OrderService.Api.IntegrationEvents.Events;

namespace OrderService.Api.IntegrationEvents.EventHandlers;

public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    public Task Handle(OrderStartedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}