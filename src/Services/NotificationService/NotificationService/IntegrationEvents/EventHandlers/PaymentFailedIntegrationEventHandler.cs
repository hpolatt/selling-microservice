using System;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.Events;

namespace NotificationService.IntegrationEvents.EventHandlers;

public class PaymentFailedIntegrationEventHandler : IIntegrationEventHandler<PaymentFailedIntegrationEvent>
{

    private readonly ILogger<PaymentFailedIntegrationEventHandler> logger;

    public PaymentFailedIntegrationEventHandler(ILogger<PaymentFailedIntegrationEventHandler> logger)
    {
        this.logger = logger;
    }
    public Task Handle(PaymentFailedIntegrationEvent @event)
    {
        // Send email to customer or notify the customer in some way
        logger.LogInformation($"Payment failed for order {@event.OrderId}. Reason: {@event.ErrorMessage}");
        return Task.CompletedTask;
    }
}
