using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;

namespace OrderService.Infrastructure.EntityConfigurations;

public class BuyerEntitiyConfiguration : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        builder.ToTable("buyers");

        builder.HasKey(b => b.Id);

        builder.Ignore(b => b.DomainEvents);

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(b=> b.Name).HasColumnType("name").HasColumnType("varchar").HasMaxLength(100);

        builder.HasMany(b => b.PaymentMethods)
            .WithOne()
            .HasForeignKey("BuyerId")
            .OnDelete(DeleteBehavior.Cascade);

        var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
