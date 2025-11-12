using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Authentication.Commands;
using RozeCare.Domain.Enums;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResult>> Register([FromBody] RegisterRequest request)
    {
        var role = Enum.TryParse<UserRole>(request.Role, true, out var parsed) ? parsed : UserRole.Patient;
        var result = await _mediator.Send(new RegisterUserCommand(request.Email, request.Password, request.Name, role));
        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResult>> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResult>> Refresh([FromBody] RefreshRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? request.UserId.ToString());
        var result = await _mediator.Send(new RefreshTokenCommand(userId, request.RefreshToken));
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? request.UserId.ToString());
        await _mediator.Send(new LogoutCommand(userId, request.RefreshToken));
        return NoContent();
    }
}

public record RegisterRequest(string Email, string Password, string Name, string Role);
public record LoginRequest(string Email, string Password);
public record RefreshRequest(Guid UserId, string RefreshToken);
