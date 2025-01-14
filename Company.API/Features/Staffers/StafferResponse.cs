using WireOps.Company.Application.Staffers;

namespace WireOps.Company.API.Features.Staffers;

public record StafferResponse(Guid Id, string Email, string GivenName, string FamilyName, string? UserId)
{
    public static StafferResponse FromModel(StafferModel model) => new(model.Id, model.Email, model.GivenName, model.FamilyName, model.UserId);
}
