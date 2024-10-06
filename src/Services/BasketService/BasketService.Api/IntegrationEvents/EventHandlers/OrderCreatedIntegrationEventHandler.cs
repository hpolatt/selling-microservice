using System;
using BasketService.Api.Core.Application.Repository;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base.Abstraction;

namespace BasketService.Api.IntegrationEvents.EventHandlers;

public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly IBasketRepository basketRepository;
    private readonly ILogger<OrderCreatedIntegrationEvent> logger;

    public OrderCreatedIntegrationEventHandler(IBasketRepository basketRepository, ILogger<OrderCreatedIntegrationEvent> logger)
    {
        this.basketRepository = basketRepository;
        this.logger = logger;
    }
    public async Task Handle(OrderCreatedIntegrationEvent @event)
    {
        logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "BasketService.Api", @event);
        await basketRepository.DeleteBasketAsync(@event.UserId.ToString());
    }
}
