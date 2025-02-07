using Microsoft.AspNetCore.Authorization;

namespace WireOps.Business.Application.Auth;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      HasScopeRequirement requirement
    )
    {
        // If user does not have the scope claim, get out of here
        if (!context.User.HasClaim(c => c.Type == "user_claims" && c.Issuer == requirement.Issuer))
            return Task.CompletedTask;

        // Split the scopes string into an array
        var scopes = context.User
          .FindFirst(c => c.Type == "user_claims" && c.Issuer == requirement.Issuer).Value.Split(' ');

        // Succeed if the scope array contains the required scope
        if (scopes.Any(s => s == requirement.Scope) || scopes.Any(s => s == "admin"))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
