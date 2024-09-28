using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTest.Events.EventHandlers;
using EventBus.UnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

[TestClass]
public class EventBusTest
{
    private ServiceCollection services;

    public EventBusTest()
    {
        this.services = new ServiceCollection();
        services.AddLogging(configure => configure.AddConsole());
    }

    [TestMethod]
    public void SubscribeEventOnRabbitMQTest()
    {
        // Create a connection to RabbitMQ and declare the queue
        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "EventBus.UnitTest",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        services.AddSingleton<IEventBus>(sp => EventBusFactory.Create(GetRabbitMQConfig(), sp));

        var sp = services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        Task.Delay(90*1000).Wait();

        // eventBus.Unsubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
    }

    // [TestMethod] I don't have a local Azure Service Bus to test this at the moment
    public void SubscribeEventOnAzureTest()
    {
        // Create a connection to Azure and declare the queue
        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "EventBus.UnitTest-OrderCreated",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        services.AddSingleton<IEventBus>(sp => EventBusFactory.Create(GetAzureConfig(), sp));

        var sp = services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        eventBus.Unsubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
    }

    private EventBusConfig GetAzureConfig() {
        return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "EventBusUnitTestTopicName",
                EventBusType = EventBusType.AzureServiceBus,
                EventNameSuffix = "IntegrationEvent",
                EventBusConnectionString = "Endpoint=sb://techbuddy.servicebus.windows.net/;SharedAccessKeyName=NewPolicyForYTVideos;SharedAccessKey=7sJghGWFOXaUaRblrbzOIIf4bQk6qkbTN/SEnKjXLpE="
        
            };
    }
    private EventBusConfig GetRabbitMQConfig() {
        return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "EventBusUnitTestTopicName",
                EventBusType = EventBusType.RabbitMQ,
                EventNameSuffix = "IntegrationEvent",
                // Connection = new ConnectionFactory
                // {
                //     HostName = "localhost",
                //     Port = 5672,
                //     UserName = "guest",
                //     Password = "guest",
                // }
            };
        
        
    }

    [TestMethod]
    public void SendMessageToRabbitMQTest()
    {
        services.AddSingleton<IEventBus>(sp => EventBusFactory.Create(GetRabbitMQConfig(), sp));

        var sp = services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Publish(new OrderCreatedIntegrationEvent(1));
    }

    // [TestMethod] I don't have a local Azure Service Bus to test this at the moment
    public void SendMessageToAzureTest()
    {
        services.AddSingleton<IEventBus>(sp => EventBusFactory.Create(GetAzureConfig(), sp));

        var sp = services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Publish(new OrderCreatedIntegrationEvent(1));
    }

}