using RozeCare.Domain.Common;
using RozeCare.Domain.Enums;

namespace RozeCare.Domain.Entities;

public class Consent : BaseAuditableEntity
{
    public Guid PatientId { get; set; }
        = Guid.Empty;

    public ApplicationUser? Patient { get; set; }
        = null;

    public ConsentGranteeType GranteeType { get; set; } = ConsentGranteeType.Provider;

    public Guid GranteeId { get; set; }
        = Guid.Empty;

    public List<string> Scopes { get; set; } = new();

    public DateTime ExpiresAtUtc { get; set; } = DateTime.UtcNow.AddMonths(1);

    public ConsentStatus Status { get; set; } = ConsentStatus.Active;
}
