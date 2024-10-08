using System;
using EventBus.AzureServiceBus;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.RabbitMQ;

namespace EventBus.Factory;

public class EventBusFactory
{
    public static IEventBus Create(EventBusConfig eventBusConfig, IServiceProvider serviceProvider)
    {
        return eventBusConfig.EventBusType switch
        {
            EventBusType.AzureServiceBus => new EventBusServiceBus(eventBusConfig, serviceProvider),
            EventBusType.RabbitMQ => new EventBusRabbitMQ(eventBusConfig, serviceProvider),
            _ => throw new NotImplementedException()
        };
    }

}
