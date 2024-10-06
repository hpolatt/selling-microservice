using System;

namespace OrderService.Domain.SeedWork;

public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (left is null ^ right is null)
            return false;
        return left?.Equals(right) != false;
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !EqualOperator(left, right);

    protected abstract IEnumerable<object> GetEqualityComponents();


    public override bool Equals(object obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;
        return GetEqualityComponents().SequenceEqual((obj as ValueObject)?.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    public ValueObject GetCopy() => MemberwiseClone() as ValueObject;

    
}
