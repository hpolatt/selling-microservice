using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Infrastructure.Context;

namespace OrderService.Infrastructure.EntityConfigurations;

public class PaymentMethodEntitiyConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("paymentmethods", OrderDbContext.DEFAULT_SCHEMA);

        builder.Ignore(pm => pm.DomainEvents);

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(pm => pm.CardHolderName)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnType("CardHolderName")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pm => pm.Alias)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasColumnType("Alias")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pm => pm.CardNumber)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasColumnType("CardNumber")
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(pm => pm.Expiration)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasColumnType("Expiration")
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(pm => pm.CardTypeId)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasColumnType("CardTypeId")
            .IsRequired();



        builder.Property(pm => pm.SecurityNumber)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasColumnType("SecurityNumber")
            .HasMaxLength(10)
            .IsRequired();


        builder.HasOne(pm => pm.CardType)
            .WithMany()
            .HasForeignKey("CardTypeId");
    }
}
