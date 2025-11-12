using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Fhir.Queries;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/fhir")]
public class FhirController : ControllerBase
{
    private readonly IMediator _mediator;

    public FhirController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Observation")]
    [Authorize]
    public async Task<ActionResult<FhirBundle<FhirObservationDto>>> GetObservation([FromQuery] Guid patient)
    {
        var bundle = await _mediator.Send(new GetFhirObservationBundleQuery(patient));
        return Ok(bundle);
    }
}
