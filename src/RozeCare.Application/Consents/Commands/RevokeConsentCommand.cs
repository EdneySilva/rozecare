using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Enums;

namespace RozeCare.Application.Consents.Commands;

public record RevokeConsentCommand(Guid PatientId, Guid ConsentId) : IRequest;

public class RevokeConsentCommandValidator : AbstractValidator<RevokeConsentCommand>
{
    public RevokeConsentCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.ConsentId).NotEmpty();
    }
}

public class RevokeConsentCommandHandler : IRequestHandler<RevokeConsentCommand>
{
    private readonly IApplicationDbContext _context;

    public RevokeConsentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokeConsentCommand request, CancellationToken cancellationToken)
    {
        var consent = await _context.Consents.FirstOrDefaultAsync(c => c.PatientId == request.PatientId && c.Id == request.ConsentId, cancellationToken)
            ?? throw new InvalidOperationException("Consent not found.");

        consent.Status = ConsentStatus.Revoked;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
