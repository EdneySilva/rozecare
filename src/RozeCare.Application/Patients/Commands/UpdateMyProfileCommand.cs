using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Patients.Commands;

public record UpdateMyProfileCommand(Guid UserId, string? BloodType, IReadOnlyCollection<string> Conditions, IReadOnlyCollection<string> Allergies, IReadOnlyCollection<string> EmergencyContacts, IReadOnlyCollection<string> PreferredProviders) : IRequest;

public class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMyProfileCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.PatientProfiles.FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Profile not found.");

        profile.BloodType = request.BloodType;
        profile.Conditions = request.Conditions.ToList();
        profile.Allergies = request.Allergies.ToList();
        profile.EmergencyContacts = request.EmergencyContacts.ToList();
        profile.PreferredProviders = request.PreferredProviders.ToList();
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
