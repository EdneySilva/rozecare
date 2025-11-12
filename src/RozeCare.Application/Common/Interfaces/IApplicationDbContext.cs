using Microsoft.EntityFrameworkCore;
using RozeCare.Domain.Entities;

namespace RozeCare.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<PatientProfile> PatientProfiles { get; }
    DbSet<Provider> Providers { get; }
    DbSet<Encounter> Encounters { get; }
    DbSet<Observation> Observations { get; }
    DbSet<Medication> Medications { get; }
    DbSet<Allergy> Allergies { get; }
    DbSet<Document> Documents { get; }
    DbSet<Consent> Consents { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<UserRefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
