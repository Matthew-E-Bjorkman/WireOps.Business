using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Staffers.Get;

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
