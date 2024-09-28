using System;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.EventHandlers;
using NotificationService.IntegrationEvents.Events;

namespace NotificationService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            ConfigurationService(services);

            var sp = services.BuildServiceProvider();
            IEventBus eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Subscribe<PaymentSucceesIntegrationEvent, PaymentSucceesIntegrationEventHandler>();
            eventBus.Subscribe<PaymentFailedIntegrationEvent, PaymentFailedIntegrationEventHandler>();

            Console.WriteLine("Notifcation Service is running!");

            Console.ReadLine();
        }

        private static void ConfigurationService(ServiceCollection services)
        {
            services.AddLogging(configure => {
                configure.AddConsole();
            });

            services.AddTransient<PaymentFailedIntegrationEventHandler>();
            services.AddTransient<PaymentSucceesIntegrationEventHandler>();
            services.AddSingleton(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "PaymentService",
                    EventBusType = EventBusType.RabbitMQ
                };
                return EventBusFactory.Create(config, sp);
            });
        }
    }
}