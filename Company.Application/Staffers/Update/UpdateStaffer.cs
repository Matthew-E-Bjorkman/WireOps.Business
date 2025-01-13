using WireOps.Company.Application.Common;

namespace WireOps.Company.Application.Staffers.Update;

public readonly struct UpdateStaffer (Guid id, string name, string sku, string? description) : Command
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string SKU { get; } = sku;
    public string? Description { get; } = description;
}
