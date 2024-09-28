using System;
using EventBus.Base.Events;

namespace NotificationService.IntegrationEvents.Events;

public class PaymentSucceesIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public PaymentSucceesIntegrationEvent(int OrderId)
    {
        this.OrderId = OrderId;
    }
}
