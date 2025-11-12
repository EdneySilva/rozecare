using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Medications.Commands;
using RozeCare.Application.Medications.Queries;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/patients/{patientId:guid}/medications")]
public class MedicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<MedicationDto>>> Get(Guid patientId, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetMedicationsQuery(patientId), cancellationToken);
        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = "Clinician,OrgAdmin,Admin")]
    public async Task<ActionResult<Guid>> Post(Guid patientId, [FromBody] CreateMedicationRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateMedicationCommand(patientId, request.Name, request.Dosage, request.StartDate, request.EndDate, request.PrescribedBy), cancellationToken);
        return CreatedAtAction(nameof(Get), new { patientId }, id);
    }
}

public record CreateMedicationRequest(string Name, string Dosage, DateTime StartDate, DateTime? EndDate, string? PrescribedBy);
