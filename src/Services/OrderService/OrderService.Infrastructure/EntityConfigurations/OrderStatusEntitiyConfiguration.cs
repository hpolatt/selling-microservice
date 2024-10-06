using System;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;

namespace OrderService.Infrastructure.EntityConfigurations;

public class OrderStatusEntitiyConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("orderstatuses", OrderDbContext.DEFAULT_SCHEMA);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
