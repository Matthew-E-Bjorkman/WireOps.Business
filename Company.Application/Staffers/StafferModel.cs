using WireOps.Company.Domain.Staffers;

namespace WireOps.Company.Application.Staffers;

public class StafferModel
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public string? UserId { get; set; }
    public required string GivenName { get; set; }
    public required string FamilyName { get; set; }

    public static StafferModel MapFromAggregate(Staffer aggregate)
    {
        return new StafferModel
        {
            Id = aggregate._data.Id.Value,
            Email = aggregate._data.Email.Value,
            UserId = aggregate._data.UserId,
            GivenName = aggregate._data.GivenName,
            FamilyName = aggregate._data.FamilyName
        };
    }
}
    