using System;
using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using PaymentService.Api.IntegrationEvents.Events;

namespace PaymentService.Api.IntegrationEvents.EventHandlers;

public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    private readonly IConfiguration configuration;
    private readonly IEventBus eventBus;
    private readonly ILogger<OrderStartedIntegrationEventHandler> logger;

    public OrderStartedIntegrationEventHandler(IConfiguration configuration, IEventBus eventBus, ILogger<OrderStartedIntegrationEventHandler> logger)
    {
        this.configuration = configuration;
        this.eventBus = eventBus;
        this.logger = logger;
    }

    public Task Handle(OrderStartedIntegrationEvent @event)
    {
        string keyword = "PaymentSucess";

        bool paymentSucessFlag = configuration.GetValue<bool>(keyword);

        IntegrationEvent paymentEvent = paymentSucessFlag 
                ? new PaymentSucceesIntegrationEvent(@event.OrderId) 
                : new PaymentFailedIntegrationEvent(@event.OrderId, "Payment failed"); 

        logger.LogInformation($"PaymentService: OrderId: {@event.OrderId} PaymentSucessFlag: {paymentSucessFlag}");

        eventBus.Publish(paymentEvent);

        return Task.CompletedTask;


    }
}
