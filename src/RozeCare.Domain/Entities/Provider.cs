using RozeCare.Domain.Common;
using RozeCare.Domain.Enums;

namespace RozeCare.Domain.Entities;

public class Provider : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public ProviderType Type { get; set; } = ProviderType.Professional;

    public string Address { get; set; } = string.Empty;

    public string Contact { get; set; } = string.Empty;

    public string? Accreditation { get; set; }
        = null;

    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
}
