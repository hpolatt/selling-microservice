using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Infrastructure.Context;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderConnection")));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBuyerRepository, BuyerRepository>();
        
        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>().UseSqlServer(configuration.GetConnectionString("OrderConnection"));

        using var context = new OrderDbContext(optionsBuilder.Options, null);
        context.Database.EnsureCreated();
        context.Database.Migrate();

        return services;
    }

}
