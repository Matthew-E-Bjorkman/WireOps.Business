using WireOps.Company.Domain.Staffers;

namespace WireOps.Company.Application.Staffers;

public class StafferModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public string? Description { get; set; }

    public static StafferModel MapFromAggregate(Staffer aggregate)
    {
        return new StafferModel
        {
            Id = aggregate._data.Id.Value,
            Name = aggregate._data.Name,
            SKU = aggregate._data.SKU,
            Description = aggregate._data.Description
        };
    }
}
    