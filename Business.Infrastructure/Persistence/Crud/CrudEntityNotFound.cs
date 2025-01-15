namespace WireOps.Business.Infrastructure.Persistence.Crud;

public class CrudEntityNotFound(Type type, Guid id)
    : Exception($"Entity not found. Type: {type.FullName}, Id: {id.ToString()}")
{
    public Type Type { get; } = type;
    public Guid Id { get; } = id;
}