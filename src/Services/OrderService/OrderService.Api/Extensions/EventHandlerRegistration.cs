using System;
using OrderService.Api.IntegrationEvents.EventHandlers;

namespace OrderService.Api.Extensions;

public static class EventHandlerRegistration
{
    public static IServiceCollection ConfigureEventHandler(this IServiceCollection services)
    {
        services.AddTransient<OrderCreatedIntegrationEventHandler>();

        return services;
    }

}
