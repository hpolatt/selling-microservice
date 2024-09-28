using System;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.Events;

namespace NotificationService.IntegrationEvents.EventHandlers;

public class PaymentSucceesIntegrationEventHandler : IIntegrationEventHandler<PaymentSucceesIntegrationEvent>
{

    private readonly ILogger<PaymentSucceesIntegrationEvent> logger;

    public PaymentSucceesIntegrationEventHandler(ILogger<PaymentSucceesIntegrationEvent> logger)
    {
        this.logger = logger;
    }
    public Task Handle(PaymentSucceesIntegrationEvent @event)
    {
        // Send email to customer or notify the customer in some way
        logger.LogInformation($"Payment succeded for order {@event.OrderId}");
        return Task.CompletedTask;
    }
}
