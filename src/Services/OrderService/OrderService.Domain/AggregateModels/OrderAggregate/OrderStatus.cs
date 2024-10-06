using System;
using OrderService.Domain.Exceptions;
using OrderService.Domain.SeedWork;

namespace OrderService.Domain.AggregateModels.OrderAggregate;

public class OrderStatus : Enumeration
{
    public OrderStatus(int id, string name) : base(id, name)
    {
    }

    public static OrderStatus Submitted = new(1, nameof(Submitted));
    public static OrderStatus AwaitingValidation = new(2, nameof(AwaitingValidation));
    public static OrderStatus StockConfirmed = new(3, nameof(StockConfirmed));
    public static OrderStatus Paid = new(4, nameof(Paid));
    public static OrderStatus Shipped = new(5, nameof(Shipped));
    public static OrderStatus Cancelled = new(6, nameof(Cancelled));

    public static IEnumerable<OrderStatus> List() =>
        new[] { Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled };

    public static OrderStatus FromName(string name) {
        var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        return state ?? throw new OrderingDomainException($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}");
    }

    public static OrderStatus From(int id) {
        var state = List().SingleOrDefault(s => s.Id == id);
        return state ?? throw new OrderingDomainException($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}");
    }


}
