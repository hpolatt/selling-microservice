using System;
using EventBus.Base.Events;

namespace OrderService.Api.IntegrationEvents.Events;

public class OrderStartedIntegrationEvent: IntegrationEvent
{
    public string UserId { get; set; }
    public int OrderId { get; set; }

    public OrderStartedIntegrationEvent(string userId, int orderId)
    {
        UserId = userId;
        OrderId = orderId;
    }

}
