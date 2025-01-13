using WireOps.Company.Application.Common;
using WireOps.Company.Domain.Staffers;

namespace WireOps.Company.Application.Staffers.GetList;

public class GetStafferListHandler (
    Staffer.Repository repository
) : QueryHandler<GetStafferList, IReadOnlyList<StafferModel>>
{
    public async Task<IReadOnlyList<StafferModel>> Handle(GetStafferList query)
    {
        var staffers = await repository.GetAll();

        if (staffers == null)
        {
            return new List<StafferModel>().AsReadOnly();
        }

        return staffers.Select(StafferModel.MapFromAggregate).ToList().AsReadOnly();
    }
}
