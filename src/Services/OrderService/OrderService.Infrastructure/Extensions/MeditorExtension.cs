using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.Context;

namespace OrderService.Infrastructure.Extensions;

public static class MeditorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderDbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }

}
