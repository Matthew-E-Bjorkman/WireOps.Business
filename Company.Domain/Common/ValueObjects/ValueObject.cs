namespace WireOps.Company.Domain.Common.ValueObjects;

public interface ValueObject<T>
{
    T Value { get; init; }
}
