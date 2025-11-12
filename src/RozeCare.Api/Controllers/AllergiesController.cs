using System.Threading;
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
    public async Task<ActionResult<IReadOnlyList<AllergyDto>>> Get(Guid patientId, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetAllergiesQuery(patientId), cancellationToken);
        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = "Clinician,OrgAdmin,Admin")]
    public async Task<ActionResult<Guid>> Post(Guid patientId, [FromBody] CreateAllergyRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateAllergyCommand(patientId, request.Substance, request.Reaction, request.Severity), cancellationToken);
        return CreatedAtAction(nameof(Get), new { patientId }, id);
    }
}

public record CreateAllergyRequest(string Substance, string Reaction, string Severity);
