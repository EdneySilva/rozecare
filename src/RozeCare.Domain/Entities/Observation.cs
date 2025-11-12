using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class Observation : BaseAuditableEntity
{
    public Guid PatientId { get; set; }
        = Guid.Empty;

    public ApplicationUser? Patient { get; set; }
        = null;

    public string Code { get; set; } = string.Empty;

    public string Display { get; set; } = string.Empty;

    public string? ValueString { get; set; }
        = null;

    public decimal? ValueQuantity { get; set; }
        = null;

    public string? ValueCodeable { get; set; }
        = null;

    public string? Unit { get; set; }
        = null;

    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
}
