using System;
using EventBus.Base.Events;

namespace OrderService.Application.IntegrationEvents;

public class OrderStartedIntegrationEvent : IntegrationEvent
{

    public string UserName { get; }

    public OrderStartedIntegrationEvent(string userName)
    {
        UserName = userName;
    }
}
