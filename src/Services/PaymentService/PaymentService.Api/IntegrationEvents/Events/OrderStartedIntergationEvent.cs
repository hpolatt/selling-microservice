using System;
using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; set; }

    public OrderStartedIntegrationEvent(int orderId)
    {
        OrderId = orderId;
    }

    public OrderStartedIntegrationEvent()
    {
    }

    
}
