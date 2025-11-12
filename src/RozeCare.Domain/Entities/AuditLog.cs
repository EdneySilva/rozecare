using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class AuditLog : BaseAuditableEntity
{
    public DateTime WhenUtc { get; set; } = DateTime.UtcNow;

    public Guid? ActorUserId { get; set; }
        = null;

    public string Action { get; set; } = string.Empty;

    public string ResourceType { get; set; } = string.Empty;

    public Guid? ResourceId { get; set; }
        = null;

    public string Details { get; set; } = string.Empty;
}
