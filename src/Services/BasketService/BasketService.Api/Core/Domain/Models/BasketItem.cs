using System;
using System.ComponentModel.DataAnnotations;

namespace BasketService.Api.Core.Domain.Models;

public class BasketItem : IValidatableObject
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal OldUnitPrice { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        if (Quantity <= 0)
        {
            results.Add(new ValidationResult("Quantity must be greater than 0", new[] { nameof(Quantity) }));
        }
        return results;
    }
}
