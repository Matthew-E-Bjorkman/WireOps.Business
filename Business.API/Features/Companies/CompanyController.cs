using Business.API.Common.Records;
using Business.API.Features.Companies.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireOps.Business.Application.Common;
using WireOps.Business.Application.Companies;
using WireOps.Business.Application.Companies.Create;
using WireOps.Business.Application.Companies.Get;
using WireOps.Business.Application.Companies.Update;
using WireOps.Business.Common.Errors;

namespace WireOps.Business.API.Features.Companies;

[ApiController]
[Route("[controller]")]
public class CompanyController(
    QueryHandler<GetCompany, CompanyModel?> GetCompanyHandler,
    CommandHandler<CreateCompany, CompanyModel> CreateCompanyHandler,
    CommandHandler<UpdateCompany, CompanyModel?> UpdateCompanyHandler
) : ControllerBase
{
    [HttpPost("/company", Name = "CreateCompany")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<ActionResult> Create(CompanyCreateRequest request)
    {
        var command = new CreateCompany(request.Name, request.UserId, request.OwnerEmail, request.OwnerGivenName, request.OwnerFamilyName);

        try
        {
            var company = CompanyRecord.FromModel(await CreateCompanyHandler.Handle(command));

            return Created($"{Request.Host}/companies", company);
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }

    [HttpGet("/company/{id}", Name = "GetCompany")]
    [ProducesResponseType<CompanyModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize("read:companies")]
    public async Task<ActionResult> Get(Guid id)
    {
        var query = new GetCompany(id);

        var company = await GetCompanyHandler.Handle(query);

        return company == null ?
            NotFound() : 
            Ok(company);
    }

    [HttpPut("/company/{id}", Name = "UpdateCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize("write:companies")]
    public async Task<ActionResult> Update(Guid id, CompanyUpdateRequest request)
    {
        var command = new UpdateCompany(id, request.Name, AddressRecord.ToModel(request.Address));
        try
        {
            var companyModel = await UpdateCompanyHandler.Handle(command);

            if (companyModel == null)
            {
                return NotFound();
            }

            return Ok(CompanyRecord.FromModel(companyModel));
        }
        catch (DomainError)
        {
            return BadRequest();
        }
    }
}
