using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireOps.Business.Application.Common;
using WireOps.Business.Application.Staffers;
using WireOps.Business.Application.Staffers.Create;
using WireOps.Business.Application.Staffers.Delete;
using WireOps.Business.Application.Staffers.Get;
using WireOps.Business.Application.Staffers.GetList;
using WireOps.Business.Application.Staffers.Update;
using WireOps.Business.Common.Errors;

namespace WireOps.Business.API.Features.Staffers;

[ApiController]
[Route("[controller]")]
public class StafferController(
    QueryHandler<GetStaffer, StafferModel?> GetStafferHandler,
    QueryHandler<GetStafferList, IReadOnlyList<StafferModel>> GetStafferListHandler,
    CommandHandler<CreateStaffer, StafferModel> CreateStafferHandler,
    CommandHandler<UpdateStaffer, StafferModel?> UpdateStafferHandler,
    CommandHandler<DeleteStaffer, bool> DeleteStafferHandler,
    CommandHandler<LinkUserToStaffer, StafferModel?> LinkUserHandler,
    CommandHandler<InviteStaffer, StafferModel?> InviteStafferHandler
) : ControllerBase
{
    [HttpPost("/company/{companyId}/staffer", Name = "CreateStaffer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> Create(Guid companyId, StafferRequest request)
    {
        var command = new CreateStaffer(companyId, request.email, request.given_name, request.family_name);

        try
        {
            var stafferModel = await CreateStafferHandler.Handle(command);

            return Created($"{Request.Host}/staffers", StafferRecord.FromModel(stafferModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpGet("/company/{companyId}/staffer/{id}", Name = "GetStaffer")]
    [ProducesResponseType<StafferModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("read:staffers")]
    public async Task<ActionResult> Get(Guid companyId, Guid id)
    {
        var query = new GetStaffer(companyId, id);

        var staffer = await GetStafferHandler.Handle(query);

        return staffer == null ?
            NotFound() : 
            Ok(StafferRecord.FromModel(staffer));
    }

    [HttpGet("/company/{companyId}/staffer", Name = "GetStafferList")]
    [ProducesResponseType<StafferModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("read:staffers")]
    public async Task<ActionResult> Get(Guid companyId)
    {
        var query = new GetStafferList(companyId);

        var staffers = await GetStafferListHandler.Handle(query);

        return staffers == null ?
            NotFound() :
            Ok(staffers.Select(StafferRecord.FromModel));
    }

    [HttpPut("/company/{companyId}/staffer/{id}", Name = "UpdateStaffer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> Update(Guid companyId, Guid id, StafferRequest request)
    {
        var command = new UpdateStaffer(companyId, id, request.email, request.given_name, request.family_name);
        try
        {
            var stafferModel = await UpdateStafferHandler.Handle(command);

            if (stafferModel == null)
            {
                return NotFound();
            }

            return Ok(StafferRecord.FromModel(stafferModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpDelete("/company/{companyId}/staffer/{id}", Name = "DeleteStaffer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> Delete(Guid companyId, Guid id)
    {
        var command = new DeleteStaffer(companyId, id);
        return await DeleteStafferHandler.Handle(command) ?
            Ok() :
            NotFound();
    }

    [HttpPost("/company/{companyId}/staffer/{id}/user-link", Name = "CreateStafferUserLink")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> LinkUser(Guid companyId, Guid id, string userId)
    {
        var command = new LinkUserToStaffer(companyId, id, userId);
        try
        {
            var stafferModel = await LinkUserHandler.Handle(command);

            if (stafferModel == null)
            {
                return NotFound();
            }

            return Ok(StafferRecord.FromModel(stafferModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpPost("/company/{companyId}/staffer/{id}/invite", Name = "CreateStafferInvite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> Invite(Guid companyId, Guid id)
    {
        var command = new InviteStaffer(companyId, id);
        try
        {
            var stafferModel = await InviteStafferHandler.Handle(command);

            if (stafferModel == null)
            {
                return NotFound();
            }

            return Ok(StafferRecord.FromModel(stafferModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }
}
