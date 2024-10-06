using System;
using System.ComponentModel.DataAnnotations;
using OrderService.Domain.SeedWork;

namespace OrderService.Domain.AggregateModels.OrderAggregate;

public class OrderItem : BaseEntity, IValidatableObject
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string PictureUrl { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Units { get; private set; }
    
    public OrderItem()
    {
    }

    public OrderItem(int productId, string productName, decimal unitPrice, int units, string pictureUrl)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Units = units;
        PictureUrl = pictureUrl;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        if (Units <= 0)
        {
            results.Add(new ValidationResult("Invalid number of units", new[] { nameof(Units) }));
        }
        return results;
    }
}