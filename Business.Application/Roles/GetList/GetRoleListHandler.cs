using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Application.Roles.GetList;

public class GetRoleListHandler(
    Role.Repository repository
) : QueryHandler<GetRoleList, IReadOnlyList<RoleModel>>
{
    public async Task<IReadOnlyList<RoleModel>> Handle(GetRoleList query)
    {
        var roles = await repository.GetAllForCompany(CompanyId.From(query.CompanyId));

        if (roles == null)
        {
            return new List<RoleModel>().AsReadOnly();
        }

        return roles.Select(RoleModel.MapFromAggregate).ToList().AsReadOnly();
    }
}

