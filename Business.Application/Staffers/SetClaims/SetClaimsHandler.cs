using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireOps.Business.Application.Auth;
using WireOps.Business.Application.Common;
using WireOps.Business.Common.Errors;

namespace WireOps.Business.Application.Staffers.SetClaims;

public class SetClaimsHandler(
    Auth0APIClient auth0APIClient
) : CommandHandler<SetClaims, bool>
{
    public async Task<bool> Handle(SetClaims command)
    {
        var claims = command.IsAdmin ? new List<string> { "admin" } : command.Permissions.Select(p => $"{p.Action}:{p.Resource}");

        if (string.IsNullOrEmpty(command.UserId))
        {
            throw new DomainError("Cannot set claims for an invalid user id");
        }

        return await auth0APIClient.UpdateUser(command.UserId, claims: claims) != null;
    }
}
