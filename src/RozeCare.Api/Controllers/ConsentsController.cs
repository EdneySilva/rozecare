using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Consents.Commands;
using RozeCare.Application.Consents.Queries;
using RozeCare.Domain.Enums;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/patients/{patientId:guid}/consents")]
public class ConsentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConsentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<Guid>> Post(Guid patientId, [FromBody] ConsentRequest request)
    {
        var id = await _mediator.Send(new CreateOrUpdateConsentCommand(patientId, request.GranteeType, request.GranteeId, request.Scopes, request.ExpiresAtUtc));
        return Ok(id);
    }

    [HttpGet]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<ActionResult<IReadOnlyList<ConsentDto>>> Get(Guid patientId)
    {
        var consents = await _mediator.Send(new GetConsentsQuery(patientId));
        return Ok(consents);
    }

    [HttpPost("{consentId:guid}/revoke")]
    [Authorize(Roles = "Patient,Admin")]
    public async Task<IActionResult> Revoke(Guid patientId, Guid consentId)
    {
        await _mediator.Send(new RevokeConsentCommand(patientId, consentId));
        return NoContent();
    }
}

public record ConsentRequest(ConsentGranteeType GranteeType, Guid GranteeId, IReadOnlyCollection<string> Scopes, DateTime ExpiresAtUtc);
