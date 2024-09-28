using System;
using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events;

public class PaymentSucceesIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public PaymentSucceesIntegrationEvent(int OrderId)
    {
        this.OrderId = OrderId;
    }
}
