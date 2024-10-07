using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Infrastructure.EntityConfigurations;

public class OrderItemEntitiyConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("orderitems");

        builder.HasKey(oi => oi.Id);

        builder.Ignore(oi => oi.DomainEvents);

        builder.Property(oi => oi.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property<int>("OrderId").IsRequired();


    }
}
