using Microsoft.AspNetCore.Mvc;
using WireOps.Business.Application.Common;
using WireOps.Business.Application.Roles.Create;
using WireOps.Business.Application.Roles.Delete;
using WireOps.Business.Application.Roles.Get;
using WireOps.Business.Application.Roles.GetList;
using WireOps.Business.Application.Roles.Update;
using WireOps.Business.Application.Roles;
using Microsoft.AspNetCore.Authorization;
using WireOps.Business.Common.Errors;

namespace WireOps.Business.API.Features.Roles;

[ApiController]
[Route("[controller]")]
public class RoleController (
    QueryHandler<GetRole, RoleModel?> GetRoleHandler,
    QueryHandler<GetRoleList, IReadOnlyList<RoleModel>> GetRoleListHandler,
    CommandHandler<CreateRole, RoleModel> CreateRoleHandler,
    CommandHandler<UpdateRole, RoleModel?> UpdateRoleHandler,
    CommandHandler<DeleteRole, bool> DeleteRoleHandler
) : ControllerBase
{
    [HttpPost("/company/{companyId}/role", Name = "CreateRole")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:roles")]
    public async Task<ActionResult> Create(Guid companyId, RoleRequest request)
    {
        var command = new CreateRole(companyId, request.name, request.is_admin, request.permissions?.Select(PermissionRecord.ToModel));

        try
        {
            var roleModel = await CreateRoleHandler.Handle(command);

            return Created($"{Request.Host}/roles", RoleRecord.FromModel(roleModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpGet("/company/{companyId}/role/{id}", Name = "GetRole")]
    [ProducesResponseType<RoleModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("read:roles")]
    public async Task<ActionResult> Get(Guid companyId, Guid id)
    {
        var query = new GetRole(companyId, id);

        var role = await GetRoleHandler.Handle(query);

        return role == null ?
            NotFound() :
            Ok(RoleRecord.FromModel(role));
    }

    [HttpGet("/company/{companyId}/role", Name = "GetRoleList")]
    [ProducesResponseType<RoleModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("read:roles")]
    public async Task<ActionResult> Get(Guid companyId)
    {
        var query = new GetRoleList(companyId);

        var roles = await GetRoleListHandler.Handle(query);

        return roles == null ?
            NotFound() :
            Ok(roles.Select(RoleRecord.FromModel));
    }

    [HttpPut("/company/{companyId}/role/{id}", Name = "UpdateRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:roles")]
    public async Task<ActionResult> Update(Guid companyId, Guid id, RoleRequest request)
    {
        var command = new UpdateRole(companyId, id, request.name, request.is_admin, request.permissions?.Select(PermissionRecord.ToModel));
        try
        {
            var roleModel = await UpdateRoleHandler.Handle(command);

            if (roleModel == null)
            {
                return NotFound();
            }

            return Ok(RoleRecord.FromModel(roleModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpDelete("/company/{companyId}/role/{id}", Name = "DeleteRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("write:roles")]
    public async Task<ActionResult> Delete(Guid companyId, Guid id)
    {
        var command = new DeleteRole(companyId, id);
        return await DeleteRoleHandler.Handle(command) ?
            Ok() :
            NotFound();
    }
}
