using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Allergies.Commands;
using RozeCare.Application.Allergies.Queries;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/patients/{patientId:guid}/allergies")]
public class AllergiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AllergiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<AllergyDto>>> Get(Guid patientId)
    {
        var items = await _mediator.Send(new GetAllergiesQuery(patientId));
        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = "Clinician,OrgAdmin,Admin")]
    public async Task<ActionResult<Guid>> Post(Guid patientId, [FromBody] CreateAllergyRequest request)
    {
        var id = await _mediator.Send(new CreateAllergyCommand(patientId, request.Substance, request.Reaction, request.Severity));
        return CreatedAtAction(nameof(Get), new { patientId }, id);
    }
}

public record CreateAllergyRequest(string Substance, string Reaction, string Severity);
