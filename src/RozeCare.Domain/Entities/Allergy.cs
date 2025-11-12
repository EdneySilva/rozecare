using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class Allergy : BaseAuditableEntity
{
    public Guid PatientId { get; set; }
        = Guid.Empty;

    public ApplicationUser? Patient { get; set; }
        = null;

    public string Substance { get; set; } = string.Empty;

    public string Reaction { get; set; } = string.Empty;

    public string Severity { get; set; } = string.Empty;
}
