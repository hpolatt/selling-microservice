using System;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApllicationRegisteration(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        return services;
    }

}
