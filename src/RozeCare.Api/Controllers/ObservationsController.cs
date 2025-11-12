using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Observations.Commands;
using RozeCare.Application.Observations.Queries;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/patients/{patientId:guid}/observations")]
public class ObservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ObservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<ObservationDto>>> Get(Guid patientId, [FromQuery] string? code, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPatientObservationsQuery(patientId, code, dateFrom, dateTo), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Clinician,OrgAdmin,Admin")]
    public async Task<ActionResult<Guid>> Post(Guid patientId, [FromBody] CreateObservationRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreateObservationCommand(patientId, request.Code, request.Display, request.ValueString, request.ValueQuantity, request.ValueCodeable, request.Unit, request.EffectiveDate), cancellationToken);
        return CreatedAtAction(nameof(Get), new { patientId }, id);
    }
}

public record CreateObservationRequest(string Code, string Display, string? ValueString, decimal? ValueQuantity, string? ValueCodeable, string? Unit, DateTime EffectiveDate);
