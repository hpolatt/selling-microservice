using System;
using System.Text;
using EventBus.Base;
using EventBus.Base.Events;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventBus.AzureServiceBus;

public class EventBusServiceBus : BaseEventBus
{
    private ITopicClient topicClient;
    private ManagementClient managementClient;
    private ILogger logger;
    public EventBusServiceBus(EventBusConfig eventBusConfig, IServiceProvider serviceProvider) : base(eventBusConfig, serviceProvider)
    {
        logger = serviceProvider.GetService(typeof(ILogger<EventBusServiceBus>)) as ILogger<EventBusServiceBus> ?? throw new ArgumentNullException(nameof(logger), "Logger service not found.");
        managementClient = new ManagementClient(eventBusConfig.EventBusConnectionString);
        topicClient = CreateTopicClient();

    }

    private ITopicClient CreateTopicClient()
    {
        if (topicClient == null || topicClient.IsClosedOrClosing)
        {
            topicClient = new TopicClient(EventBusConfig.EventBusConnectionString, EventBusConfig.DefaultTopicName);
        }
        // Ensure the topic exists
        if (!managementClient.TopicExistsAsync(EventBusConfig.DefaultTopicName).GetAwaiter().GetResult())
        {
            managementClient.CreateTopicAsync(EventBusConfig.DefaultTopicName).GetAwaiter().GetResult();
        }
        return topicClient;
    }

    public override void Publish(IntegrationEvent @event)
    {
        var eventName = @event.GetType().Name; // exp: OrderCreatedIntegrationEvent
        eventName = ProcessEventName(eventName);

        var eventStr = JsonConvert.SerializeObject(@event);
        var bodyArr = Encoding.UTF8.GetBytes(eventStr);
        var message = new Message
        {
            MessageId = Guid.NewGuid().ToString(),
            Body = bodyArr,
            Label = eventName
        };

        topicClient.SendAsync(message).GetAwaiter().GetResult();
    }

    public override void Subscribe<T, TH>()
    {
        var eventName = typeof(T).Name;
        eventName = ProcessEventName(eventName);
        if (!SubscriptionManager.HasSubscriptionsForEvent(eventName)) {
            var subscriptionClient = CreateSubscriptionClientIfNotExist(eventName);
            RegisterSubscriptionClientMessageHandler(subscriptionClient);
        }

        logger.LogWarning($"Subscribing to event {eventName} with {typeof(TH).Name}");
        SubscriptionManager.AddSubscription<T, TH>();
    }

    public override void Unsubscribe<T, TH>()
    {
        var eventName = typeof(T).Name;
        try
        {
            var subscriptionClient = CreateSubscriptionClient(eventName);
            subscriptionClient.RemoveRuleAsync(eventName).GetAwaiter().GetResult();
        }
        catch (MessagingEntityNotFoundException)
        {
            logger.LogWarning("The messaging entity {eventName} Could not be found.", eventName);
        }
        logger.LogWarning("Unsubscribing from event {eventName}", eventName);
        SubscriptionManager.RemoveSubscription<T, TH>();
    }

    private void RegisterSubscriptionClientMessageHandler(ISubscriptionClient subscriptionClient)
    {
        subscriptionClient.RegisterMessageHandler(async (message, token) =>
        {
            var eventName = $"{message.Label}";
            var messageData = Encoding.UTF8.GetString(message.Body);
            if (await ProcessEvent(eventName, messageData))
            {
                await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }

        }, new MessageHandlerOptions(ExceptionReceivedHandler) { AutoComplete = false });
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
    {
        logger.LogError(args.Exception, "ERROR handling messgae: {ExceptionMessage} - Context: {@ExceptionContext}", args.Exception.Message, args.ExceptionReceivedContext);
        return Task.CompletedTask;
    }

    private ISubscriptionClient CreateSubscriptionClientIfNotExist(string eventName)
    {
        var subClient = CreateSubscriptionClient(eventName);
        var exist = managementClient.SubscriptionExistsAsync(EventBusConfig.DefaultTopicName, GetSubName(eventName)).GetAwaiter().GetResult();
        if (!exist)
        {
            managementClient.CreateSubscriptionAsync(EventBusConfig.DefaultTopicName, GetSubName(eventName)).GetAwaiter().GetResult();
            RemoveDefaultRule(subClient);
        }
        CreateRuleIfNotExist(ProcessEventName(eventName), subClient);
        return subClient;
    }

    private void CreateRuleIfNotExist(string eventName, ISubscriptionClient subscriptionClient)
    {
        bool ruleExist;
        try
        {
            var rule = managementClient.GetRuleAsync(EventBusConfig.DefaultTopicName, GetSubName(eventName), eventName).GetAwaiter().GetResult();
            ruleExist = rule != null;
        }
        catch (MessagingEntityNotFoundException)
        {
            // Azure Managment Client doesn't have RuleExists Method, so we need to catch the exception
            ruleExist = false;
        }

        if (ruleExist) return;
        subscriptionClient.AddRuleAsync(new RuleDescription
        {
            Filter = new CorrelationFilter { Label = eventName },
            Name = eventName
        }).GetAwaiter().GetResult();


    }

    private void RemoveDefaultRule(SubscriptionClient subscriptionClient)
    {
        try
        {
            subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName).GetAwaiter().GetResult();
        }
        catch (MessagingEntityNotFoundException)
        {
            logger.LogWarning($"The messaging entity {RuleDescription.DefaultRuleName} Could not be found.");
        }
    }

    private SubscriptionClient CreateSubscriptionClient(string eventName)
    {
        return new SubscriptionClient(EventBusConfig.EventBusConnectionString, EventBusConfig.DefaultTopicName, GetSubName(eventName));
    }

    public override void Dispose()
    {
        base.Dispose();
        topicClient?.CloseAsync().GetAwaiter().GetResult();
        managementClient?.CloseAsync().GetAwaiter().GetResult();
        topicClient = null;
        managementClient = null;

    }
}

