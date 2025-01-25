using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Staffers.GetList;

public class GetStafferListHandler (
    Staffer.Repository repository
) : QueryHandler<GetStafferList, IReadOnlyList<StafferModel>>
{
    public async Task<IReadOnlyList<StafferModel>> Handle(GetStafferList query)
    {
        var staffers = await repository.GetAllForCompany(CompanyId.From(query.CompanyId));

        if (staffers == null)
        {
            return new List<StafferModel>().AsReadOnly();
        }

        return staffers.Select(StafferModel.MapFromAggregate).ToList().AsReadOnly();
    }
}
