﻿using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles.Events;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Application.Roles.Delete;

public class DeleteRoleHandler(
    Role.Repository repository
) : CommandHandler<DeleteRole, bool>
{
    public async Task<bool> Handle(DeleteRole command)
    {
        var role = await repository.GetBy(CompanyId.From(command.CompanyId), RoleId.From(command.Id));

        if (role == null)
        {
            return false;
        }

        await repository.Delete(role);

        return true;
    }
}