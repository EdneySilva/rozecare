using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Application.Common.Models;
using RozeCare.Application.Patients.Commands;
using RozeCare.Application.Patients.Queries;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public PatientsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("me")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<PatientProfileDto?>> GetMe()
    {
        if (_currentUser.UserId is null)
        {
            return Unauthorized();
        }

        var profile = await _mediator.Send(new GetMyProfileQuery(_currentUser.UserId.Value));
        return Ok(profile);
    }

    [HttpPut("me")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequest request)
    {
        if (_currentUser.UserId is null)
        {
            return Unauthorized();
        }

        await _mediator.Send(new UpdateMyProfileCommand(_currentUser.UserId.Value, request.BloodType, request.Conditions, request.Allergies, request.EmergencyContacts, request.PreferredProviders));
        return NoContent();
    }

    [HttpGet("{patientId:guid}")]
    [Authorize(Roles = "Clinician,OrgAdmin,Admin")]
    public async Task<ActionResult<PatientProfileDto?>> GetPatientById(Guid patientId)
    {
        var profile = await _mediator.Send(new GetPatientProfileQuery(patientId));
        return profile is null ? NotFound() : Ok(profile);
    }
}

public record UpdateProfileRequest(string? BloodType, IReadOnlyCollection<string> Conditions, IReadOnlyCollection<string> Allergies, IReadOnlyCollection<string> EmergencyContacts, IReadOnlyCollection<string> PreferredProviders);
