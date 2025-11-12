using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Enums;
using RozeCare.Infrastructure.Persistence;

namespace RozeCare.Infrastructure.Services;

public class ConsentService : IConsentService
{
    private readonly ApplicationDbContext _context;

    public ConsentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasConsentAsync(Guid patientId, Guid actorId, string scope, CancellationToken cancellationToken = default)
    {
        var consents = await _context.Consents.AsNoTracking()
            .Where(c => c.PatientId == patientId
                        && c.Status == ConsentStatus.Active
                        && c.ExpiresAtUtc >= DateTime.UtcNow
                        && c.GranteeId == actorId)
            .ToListAsync(cancellationToken);

        return consents.Any(c => c.Scopes.Contains(scope));
    }
}
