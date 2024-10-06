using System;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.EntityConfigurations;
using OrderService.Infrastructure.Extensions;

namespace OrderService.Infrastructure.Context;

public class OrderDbContext: DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "ordering";
    private readonly IMediator mediator;

    public OrderDbContext(): base()
    {
    }
    public OrderDbContext(DbContextOptions<OrderDbContext> options, IMediator mediator): base(options)
    {
        this.mediator = mediator;
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<PaymentMethod> Paymentss { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<CardType> CardTypes { get; set; }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this);
        await base.SaveChangesAsync(cancellationToken);
        
        return true;

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntitiyConfiguration());
        modelBuilder.ApplyConfiguration(new OrderStatusEntitiyConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodEntitiyConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntitiyConfiguration());
        modelBuilder.ApplyConfiguration(new CardTypeEntitiyConfiguration());

    }

}
