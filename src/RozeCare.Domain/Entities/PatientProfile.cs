using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class PatientProfile : BaseAuditableEntity
{
    public Guid UserId { get; set; }
        = Guid.Empty;

    public ApplicationUser? User { get; set; }
        = null;

    public string? BloodType { get; set; }
        = null;

    public List<string> Conditions { get; set; } = new();

    public List<string> Allergies { get; set; } = new();

    public List<string> PreferredProviders { get; set; } = new();

    public List<string> EmergencyContacts { get; set; } = new();
}
