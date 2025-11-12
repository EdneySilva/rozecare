using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Providers.Commands;
using RozeCare.Application.Providers.Queries;
using RozeCare.Domain.Enums;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/providers")]
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProvidersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<ProviderDto>>> Search([FromQuery] string? text)
    {
        var providers = await _mediator.Send(new SearchProvidersQuery(text));
        return Ok(providers);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,OrgAdmin")]
    public async Task<ActionResult<Guid>> Post([FromBody] ProviderRequest request)
    {
        var id = await _mediator.Send(new CreateProviderCommand(request.Name, request.Type, request.Address, request.Contact, request.Accreditation));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ProviderDto?>> GetById(Guid id)
    {
        var provider = await _mediator.Send(new GetProviderByIdQuery(id));
        return provider is null ? NotFound() : Ok(provider);
    }
}

public record ProviderRequest(string Name, ProviderType Type, string Address, string Contact, string? Accreditation);
