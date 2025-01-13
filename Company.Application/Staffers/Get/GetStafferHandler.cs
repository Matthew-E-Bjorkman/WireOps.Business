using WireOps.Company.Application.Common;
using WireOps.Company.Domain.Staffers;

namespace WireOps.Company.Application.Staffers.Get;

public class GetStafferHandler (
    Staffer.Repository repository
) : QueryHandler<GetStaffer, StafferModel?>
{
    public async Task<StafferModel?> Handle(GetStaffer query)
    {
        var staffer = await repository.GetBy(StafferId.From(query.Id));

        if (staffer == null)
        {
            return null;
        }

        return StafferModel.MapFromAggregate(staffer);
    }
}
