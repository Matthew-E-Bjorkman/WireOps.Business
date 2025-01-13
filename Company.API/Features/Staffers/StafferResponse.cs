using WireOps.Company.Application.Staffers;

namespace WireOps.Company.API.Features.Staffers;

public record StafferResponse(Guid Id, string Name, string SKU, string? Description)
{
    public static StafferResponse FromModel(StafferModel model) => new(model.Id, model.Name, model.SKU, model.Description);
}
