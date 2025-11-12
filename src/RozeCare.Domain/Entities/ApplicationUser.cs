using Microsoft.AspNetCore.Identity;
using RozeCare.Domain.Enums;
using RozeCare.Domain.ValueObjects;

namespace RozeCare.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Patient;

    public DateOnly? BirthDate { get; set; }
        = null;

    public CountryCode? Country { get; set; }
        = null;

    public DateTime? LgpdConsentAtUtc { get; set; }
        = null;

    public ICollection<PatientProfile> PatientProfiles { get; set; } = new List<PatientProfile>();

    public ICollection<Consent> Consents { get; set; } = new List<Consent>();

    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();
}
