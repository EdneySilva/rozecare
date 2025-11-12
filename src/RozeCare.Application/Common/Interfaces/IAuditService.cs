namespace RozeCare.Application.Common.Interfaces;

public interface IAuditService
{
    Task LogAsync(Guid? actorId, string action, string resourceType, Guid? resourceId, object details, CancellationToken cancellationToken = default);
}
