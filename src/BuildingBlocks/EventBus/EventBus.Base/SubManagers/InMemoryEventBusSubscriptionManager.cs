using System;
using EventBus.Base.Abstraction;
using EventBus.Base.Events;

namespace EventBus.Base.SubManagers;

public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> handlers;
    private readonly List<Type> eventTypes;
    public event EventHandler<string> OnEventRemoved;
    public Func<string, string> EventNameGetter;

    public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
    {
        this.handlers = new Dictionary<string, List<SubscriptionInfo>>();
        this.eventTypes = new List<Type>();
        this.EventNameGetter = eventNameGetter;
    }

    public bool IsEmpty => !handlers.Keys.Any();

    public void Clear() => handlers.Clear();


    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        string eventName = GetEventKey<T>();
        AddSubscription(typeof(TH), eventName);
        if (!eventTypes.Contains(typeof(T))) eventTypes.Add(typeof(T));
    }

    private void AddSubscription(Type handlerType, string eventName)
    {
        if (!HasSubscriptionsForEvent(eventName)) handlers.Add(eventName, new List<SubscriptionInfo>());
        if (handlers[eventName].Any(s => s.HandlerType == handlerType)) throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
        handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
    }


    public string GetEventKey<T>() => EventNameGetter(typeof(T).Name);

    public Type GetEventTypeByName(string eventName) {
        return eventTypes.SingleOrDefault(t => t.Name == eventName);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
    {
        string eventName = GetEventKey<T>();
        return GetHandlersForEvent(eventName);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => handlers[eventName];

    public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
    {
        string key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    public bool HasSubscriptionsForEvent(string eventName) => handlers.ContainsKey(eventName);

    public void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        string eventName = GetEventKey<T>();
        SubscriptionInfo handlerToRemove = FindSubscriptionToRemove<T, TH>(eventName);
        RemoveHandler(eventName, handlerToRemove);
    }

    private void RemoveHandler(string eventName, SubscriptionInfo handlerToRemove)
    {
        if (handlerToRemove is null) return;
        handlers[eventName].Remove(handlerToRemove);
        if (handlers[eventName].Any()) return;
        
        handlers.Remove(eventName);
        var eventType = eventTypes.SingleOrDefault(e => e.Name == eventName);
        if (eventType != null) eventTypes.Remove(eventType);
        RaiseOnEventRemoved(eventName);
        
    }

    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }

    private SubscriptionInfo FindSubscriptionToRemove<T, TH>(string eventName)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        return FindSubscriptionToRemove(eventName, typeof(TH));
    }

    private SubscriptionInfo FindSubscriptionToRemove(string eventName, Type type)
    {
        if (!HasSubscriptionsForEvent(eventName)) return null;
        return handlers[eventName].Single(s => s.HandlerType == type);
    }
}
