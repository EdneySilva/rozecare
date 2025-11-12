using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class Encounter : BaseAuditableEntity
{
    public Guid PatientId { get; set; }
        = Guid.Empty;

    public ApplicationUser? Patient { get; set; }
        = null;

    public Guid ProviderId { get; set; }
        = Guid.Empty;

    public Provider? Provider { get; set; }
        = null;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string Type { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public List<string> Diagnoses { get; set; } = new();

    public List<string> Prescriptions { get; set; } = new();
}
