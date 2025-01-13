namespace WireOps.Company.Infrastructure.Persistence.Crud.Results;

public readonly struct Deleted(Guid id, bool wasPresent) : IEquatable<Deleted>
{
    public Guid Id { get; } = id;
    public bool WasPresent { get; } = wasPresent;

    public bool Equals(Deleted other) => (Id, WasPresent).Equals((other.Id, other.WasPresent));

    public override bool Equals(object obj) => obj is Deleted other && Equals(other);

    public override int GetHashCode() => (Id, WasPresent).GetHashCode();
}