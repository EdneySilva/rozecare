using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Audit.Queries;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AuditController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<AuditLogDto>>> Get([FromQuery] Guid? patientId, [FromQuery] Guid? actor, [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserRole == "Patient")
        {
            patientId = _currentUserService.UserId;
        }

        var logs = await _mediator.Send(new GetAuditLogsQuery(patientId, actor, from, to), cancellationToken);
        return Ok(logs);
    }
}
