using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Staffers;

public class StafferModel
{
    public required Guid Id { get; set; }
    public required Guid CompanyId { get; set; }
    public required string Email { get; set; }
    public string? UserId { get; set; }
    public required string GivenName { get; set; }
    public required string FamilyName { get; set; }
    public required bool IsOwner { get; set; }

    public static StafferModel MapFromAggregate(Staffer aggregate)
    {
        return new StafferModel
        {
            Id = aggregate._data.Id.Value,
            CompanyId = aggregate._data.CompanyId.Value,
            Email = aggregate._data.Email.Value,
            UserId = aggregate._data.UserId,
            GivenName = aggregate._data.GivenName,
            FamilyName = aggregate._data.FamilyName,
            IsOwner = aggregate._data.IsOwner
        };
    }
}
    