using System;
using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBus.Base.Events;

public abstract class BaseEventBus : IEventBus
{
    public readonly IEventBusSubscriptionManager SubscriptionManager;
    private readonly IServiceProvider serviceProvider;
    public EventBusConfig EventBusConfig { get; private set; }

    public BaseEventBus(EventBusConfig eventBusConfig, IServiceProvider serviceProvider)
    {
        this.EventBusConfig = eventBusConfig;
        this.serviceProvider = serviceProvider;
        this.SubscriptionManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }

    public virtual string ProcessEventName(string eventName)
    {
        if (EventBusConfig.DeleteEventPrefix) eventName = eventName.TrimStart(EventBusConfig.EventNamePrefix.ToArray());
        if (EventBusConfig.DeleteEventSuffix) eventName = eventName.TrimEnd(EventBusConfig.EventNameSuffix.ToArray());

        return eventName;
    }

    public virtual string GetSubName(string eventName) => $"{EventBusConfig.SubscriberClientAppName}-{ProcessEventName(eventName)}";

    public virtual void Dispose() {
        EventBusConfig = null;
        SubscriptionManager.Clear();
    }

    public async Task<bool> ProcessEvent(string eventName, string message)
    {
        eventName = ProcessEventName(eventName);
        if (!SubscriptionManager.HasSubscriptionsForEvent(eventName)) return false;
        var subscriptions = SubscriptionManager.GetHandlersForEvent(eventName);
        using (var scope = serviceProvider.CreateScope())
        {
            foreach (var subscription in subscriptions)
            {
                var handler = serviceProvider.GetService(subscription.HandlerType);
                if (handler == null) continue;
                
                var eventType = SubscriptionManager.GetEventTypeByName($"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}");
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
            }
        }
       
        return true;
    }

    public abstract  void Publish(IntegrationEvent @event);

    public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    
    public abstract  void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
}
