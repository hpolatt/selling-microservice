using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;

namespace OrderService.Infrastructure.EntityConfigurations;

public class PaymentMethodEntitiyConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("paymentmethods");

        builder.Ignore(pm => pm.DomainEvents);

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(pm => pm.CardHolderName)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pm => pm.CardTypeAlias)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pm => pm.CardNumber)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(pm => pm.Expiration)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(pm => pm.CardTypeId)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .IsRequired();



        builder.Property(pm => pm.SecurityNumber)
            .UsePropertyAccessMode(PropertyAccessMode.Field) 
            .HasMaxLength(10)
            .IsRequired();


        builder.HasOne(pm => pm.CardType)
            .WithMany()
            .HasForeignKey("CardTypeId");
    }
}
