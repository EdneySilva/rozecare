using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Encounters.Commands;
using RozeCare.Application.Encounters.Queries;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/patients/{patientId:guid}/encounters")]
public class EncountersController : ControllerBase
{
    private readonly IMediator _mediator;

    public EncountersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<EncounterDto>>> Get(Guid patientId)
    {
        var encounters = await _mediator.Send(new GetEncountersQuery(patientId));
        return Ok(encounters);
    }

    [HttpPost]
    [Authorize(Roles = "Clinician,OrgAdmin,Admin")]
    public async Task<ActionResult<Guid>> Post(Guid patientId, [FromBody] CreateEncounterRequest request)
    {
        var id = await _mediator.Send(new CreateEncounterCommand(patientId, request.ProviderId, request.Date, request.Type, request.Notes, request.Diagnoses, request.Prescriptions));
        return CreatedAtAction(nameof(Get), new { patientId }, id);
    }
}

public record CreateEncounterRequest(Guid ProviderId, DateTime Date, string Type, string Notes, IReadOnlyCollection<string> Diagnoses, IReadOnlyCollection<string> Prescriptions);
