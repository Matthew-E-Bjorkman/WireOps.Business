using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WireOps.Company.Domain.Common.ValueObjects;

namespace WireOps.Company.Infrastructure.ObjectRelationalMapping.EntityFramework;


public class ValueObjectConverter<TValueObject, TValue> : ValueConverter<TValueObject, TValue>
    where TValueObject : ValueObject<TValue>, new()
{
    public ValueObjectConverter() : base(ToValue, ToValueObject) { }

    private static Expression<Func<TValueObject, TValue>> ToValue => id => id.Value;

    private static Expression<Func<TValue, TValueObject>> ToValueObject => value => new TValueObject { Value = value };
}