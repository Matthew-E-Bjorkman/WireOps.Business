using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Application.Roles.Get;

public class GetRoleHandler(
    Role.Repository repository
) : QueryHandler<GetRole, RoleModel?>
{
    public async Task<RoleModel?> Handle(GetRole query)
    {
        var role = await repository.GetBy(CompanyId.From(query.CompanyId), RoleId.From(query.Id));

        if (role == null)
        {
            return null;
        }

        return RoleModel.MapFromAggregate(role);
    }
}
