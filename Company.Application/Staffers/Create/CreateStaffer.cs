using WireOps.Company.Application.Common;

namespace WireOps.Company.Application.Staffers.Create;

public readonly struct CreateStaffer (string name, string sku, string? description) : Command
{
    public string Name { get; } = name;
    public string SKU { get; } = sku;
    public string? Description { get; } = description;
}
