using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class Medication : BaseAuditableEntity
{
    public Guid PatientId { get; set; }
        = Guid.Empty;

    public ApplicationUser? Patient { get; set; }
        = null;

    public string Name { get; set; } = string.Empty;

    public string Dosage { get; set; } = string.Empty;

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }
        = null;

    public string? PrescribedBy { get; set; }
        = null;
}
