﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireOps.Company.Application.Common;
using WireOps.Company.Application.Staffers;
using WireOps.Company.Application.Staffers.Create;
using WireOps.Company.Application.Staffers.Delete;
using WireOps.Company.Application.Staffers.Get;
using WireOps.Company.Application.Staffers.GetList;
using WireOps.Company.Application.Staffers.Update;
using WireOps.Company.Common.Errors;

namespace WireOps.Company.API.Features.Staffers;

[ApiController]
[Route("[controller]")]
public class StafferController(
    QueryHandler<GetStaffer, StafferModel?> GetStafferHandler,
    QueryHandler<GetStafferList, IReadOnlyList<StafferModel>> GetStafferListHandler,
    CommandHandler<CreateStaffer, StafferModel> CreateStafferHandler,
    CommandHandler<UpdateStaffer, StafferModel?> UpdateStafferHandler,
    CommandHandler<DeleteStaffer, bool> DeleteStafferHandler,
    CommandHandler<LinkUser, StafferModel?> LinkUserHandler
) : ControllerBase
{
    [HttpPost("/staffer", Name = "CreateStaffer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> Create(StafferRequest request)
    {
        var command = new CreateStaffer(request.Email, request.GivenName, request.FamilyName, request.UserId);

        try
        {
            var staffer = StafferResponse.FromModel(await CreateStafferHandler.Handle(command));

            return Created($"{Request.Host}/staffers", staffer);
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpGet("/staffer/{id}", Name = "GetStaffer")]
    [ProducesResponseType<StafferModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("read:staffers")]
    public async Task<ActionResult> Get(Guid id)
    {
        var query = new GetStaffer(id);

        var staffer = await GetStafferHandler.Handle(query);

        return staffer == null ?
            NotFound() : 
            Ok(staffer);
    }

    [HttpGet("/staffer", Name = "GetStafferList")]
    [ProducesResponseType<StafferModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("read:staffers")]
    public async Task<ActionResult> Get()
    {
        var query = new GetStafferList();

        var staffers = await GetStafferListHandler.Handle(query);

        return staffers == null ?
            NotFound() :
            Ok(staffers);
    }

    [HttpPut("/staffer/{id}", Name = "UpdateStaffer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> Update(Guid id, StafferRequest request)
    {
        var command = new UpdateStaffer(id, request.Email, request.GivenName, request.FamilyName);
        try
        {
            var stafferModel = await UpdateStafferHandler.Handle(command);

            if (stafferModel == null)
            {
                return NotFound();
            }

            return Ok(StafferResponse.FromModel(stafferModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpDelete("/staffer/{id}", Name = "DeleteStaffer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteStaffer(id);
        return await DeleteStafferHandler.Handle(command) ?
            Ok() :
            NotFound();
    }

    [HttpPost("/staffer/{id}/user-link", Name = "CreateStafferUserLink")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:staffers")]
    public async Task<ActionResult> LinkUser(Guid id, string userId)
    {
        var command = new LinkUser(id,userId);
        try
        {
            var stafferModel = await LinkUserHandler.Handle(command);

            if (stafferModel == null)
            {
                return NotFound();
            }

            return Ok(StafferResponse.FromModel(stafferModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }
}
