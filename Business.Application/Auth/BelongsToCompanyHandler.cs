using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Auth;

public class BelongsToCompanyHandler : AuthorizationHandler<BelongsToCompanyRequirement>
{
    private readonly Staffer.Repository repository;

    public BelongsToCompanyHandler(Staffer.Repository repository)
    {
        this.repository = repository;
    }

    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      BelongsToCompanyRequirement requirement
    )
    {
        var userId = context.User.Identity?.Name;

        if (userId == null)
        {
            return Task.CompletedTask;
        }

        if (userId.EndsWith("@clients"))
        {
            ValidateClientAuth(context, requirement);
        }
        else
        {
            ValidateUserAuth(context, requirement, userId);
        }


        return Task.CompletedTask;
    }

    private void ValidateClientAuth(AuthorizationHandlerContext context, BelongsToCompanyRequirement requirement)
    {
        try
        {
            var tenantId = context.User.Claims.FirstOrDefault(c => c.Type.Equals("tenant_id"))?.Value;

            if (tenantId == null)
            {
                context.Fail();
                return;
            }

            if (context.Resource is HttpContext httpContext)
            {
                var routeData = httpContext.GetRouteData();
                var companyIdAccessor = routeData.Values.ContainsKey("companyId") ? "companyId" : "id";
                var companyId = routeData.Values[companyIdAccessor].ToString();

                if (Guid.Parse(companyId!) == Guid.Parse(tenantId))
                {
                    context.Succeed(requirement);
                }
            }
        }
        catch (DomainError)
        {
            context.Fail();
        }
    }

    private void ValidateUserAuth(AuthorizationHandlerContext context, BelongsToCompanyRequirement requirement, string userId)
    {
        try
        {
            var staffer = repository.GetByUserId(userId).GetAwaiter().GetResult();

            if (context.Resource is HttpContext httpContext)
            {
                var routeData = httpContext.GetRouteData();
                var companyIdAccessor = routeData.Values.ContainsKey("companyId") ? "companyId" : "id";
                var companyId = routeData.Values[companyIdAccessor].ToString();

                if (staffer != null && Guid.Parse(companyId!) == staffer._data.CompanyId.Value)
                {
                    context.Succeed(requirement);
                }
            }
        }
        catch (DomainError)
        {
            context.Fail();
        }
    }
}
