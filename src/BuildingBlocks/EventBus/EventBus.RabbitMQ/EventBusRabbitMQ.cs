using System;
using System.Text;
using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMQ;

public class EventBusRabbitMQ : BaseEventBus
{
    RabbitMQPersistentConnection persistentConnection;
    private readonly IConnectionFactory connectionFactory;

    private IModel consumerChannel;
    public EventBusRabbitMQ(EventBusConfig eventBusConfig, IServiceProvider serviceProvider) : base(eventBusConfig, serviceProvider)
    {
        if (eventBusConfig.Connection is not null)
        {
            var connJson = JsonConvert.SerializeObject(eventBusConfig.Connection, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
        }
        else
            connectionFactory = new ConnectionFactory();

        persistentConnection = new RabbitMQPersistentConnection(connectionFactory, eventBusConfig.ConnectionRetryCount);
        consumerChannel = CreateConsumerChannal();

        SubscriptionManager.OnEventRemoved += OnEventRemoved;
    }

    private void OnEventRemoved(object? sender, string eventName)
    {
        eventName = ProcessEventName(eventName);
        if (!persistentConnection.IsConnected)
        {
            persistentConnection.TryConnect();
        }

        consumerChannel.QueueUnbind(queue: eventName, exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);

        if (SubscriptionManager.IsEmpty)
        {
            consumerChannel.Close();
        }
    }

    public override void Publish(IntegrationEvent @event)
    {
        if (!persistentConnection.IsConnected)
        {
            persistentConnection.TryConnect();
        }

        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetry(
                EventBusConfig.ConnectionRetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    // logger
                }
            );

        var eventName = @event.GetType().Name;
        eventName = ProcessEventName(eventName);

        consumerChannel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        policy.Execute(() =>
        {
            var properties = consumerChannel.CreateBasicProperties();
            properties.DeliveryMode = 2; // persistent

            // generate the published event queue
            // consumerChannel.QueueDeclare(queue: GetSubName(eventName), durable: true, exclusive: false, autoDelete: false, arguments: null);
            // consumerChannel.QueueBind(queue: GetSubName(eventName), exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);

            consumerChannel.BasicPublish(
                exchange: EventBusConfig.DefaultTopicName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body
            );
        });
    }

    public override void Subscribe<T, TH>()
    {
        var eventName = typeof(T).Name;
        eventName = ProcessEventName(eventName);

        if (!SubscriptionManager.HasSubscriptionsForEvent(eventName))
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }
            
            consumerChannel.QueueDeclare(queue: GetSubName(eventName), durable: true, exclusive: false, autoDelete: false, arguments: null);
            consumerChannel.QueueBind(queue: GetSubName(eventName), exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);

            SubscriptionManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }
    }

    public override void Unsubscribe<T, TH>()
    {
        SubscriptionManager.RemoveSubscription<T, TH>();
    }

    private IModel CreateConsumerChannal()
    {
        if (!persistentConnection.IsConnected)
            persistentConnection.TryConnect();

        var channel = persistentConnection.CreateModel();
        channel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");

        return channel;
    }

    private void StartBasicConsume(string eventName)
    {
        if (consumerChannel is not null)
        {
            var consumer = new EventingBasicConsumer(consumerChannel);
            consumer.Received += ConsumerReceived;

            consumerChannel.BasicConsume(queue: GetSubName(eventName), autoAck: false, consumer: consumer);
        }
    }

    private async void ConsumerReceived(object? sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        eventName = ProcessEventName(eventName);
        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        consumerChannel.BasicAck(eventArgs.DeliveryTag, false);

    }
}
