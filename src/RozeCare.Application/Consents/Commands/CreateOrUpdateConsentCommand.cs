using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;
using RozeCare.Domain.Enums;

namespace RozeCare.Application.Consents.Commands;

public record CreateOrUpdateConsentCommand(Guid PatientId, ConsentGranteeType GranteeType, Guid GranteeId, IReadOnlyCollection<string> Scopes, DateTime ExpiresAtUtc) : IRequest<Guid>;

public class CreateOrUpdateConsentCommandValidator : AbstractValidator<CreateOrUpdateConsentCommand>
{
    public CreateOrUpdateConsentCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.GranteeId).NotEmpty();
        RuleFor(x => x.Scopes).NotEmpty();
    }
}

public class CreateOrUpdateConsentCommandHandler : IRequestHandler<CreateOrUpdateConsentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public CreateOrUpdateConsentCommandHandler(IApplicationDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<Guid> Handle(CreateOrUpdateConsentCommand request, CancellationToken cancellationToken)
    {
        var consent = await _context.Consents
            .FirstOrDefaultAsync(c => c.PatientId == request.PatientId && c.GranteeId == request.GranteeId && c.GranteeType == request.GranteeType, cancellationToken);

        if (consent is null)
        {
            consent = new Consent
            {
                PatientId = request.PatientId,
                GranteeType = request.GranteeType,
                GranteeId = request.GranteeId,
                Scopes = request.Scopes.ToList(),
                ExpiresAtUtc = request.ExpiresAtUtc,
                Status = ConsentStatus.Active
            };
            _context.Consents.Add(consent);
        }
        else
        {
            consent.Scopes = request.Scopes.ToList();
            consent.ExpiresAtUtc = request.ExpiresAtUtc;
            consent.Status = ConsentStatus.Active;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync(request.PatientId, "consent.update", nameof(Consent), consent.Id, new { consent.GranteeId, consent.Scopes }, cancellationToken);
        return consent.Id;
    }
}
