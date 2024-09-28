using System;
using Polly;
using System.Net.Sockets;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Events;

namespace EventBus.RabbitMQ;

public class RabbitMQPersistentConnection : IDisposable
{
    private readonly IConnectionFactory connectionFactory;
    private readonly int retryCount;
    private bool disposed;
    private IConnection connection;

    private object lockObject = new object();

    public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
    {
        this.connectionFactory = connectionFactory;
        this.retryCount = retryCount;
    }

    public bool IsConnected => connection != null && connection.IsOpen;

    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return connection.CreateModel();
    }
    public void Dispose()
    {
        disposed = true;
        connection?.Dispose();
    }

    public bool TryConnect()
    {
        lock (lockObject)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                { });

            policy.Execute(() =>
            {
                connection = connectionFactory.CreateConnection();
            });
        }
        if (IsConnected)
        {
            connection.ConnectionShutdown += OnConnectionShutdown;
            connection.CallbackException += OnCallbackException;
            connection.ConnectionBlocked += OnConnectionBlocked;
            return true;
        }
            return false;
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (disposed) return;
        TryConnect();
    }

    private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if (disposed) return;
        TryConnect();
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        if (disposed) return;
        TryConnect();
    }
}
