using System.Text.Json;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;
using RozeCare.Infrastructure.Persistence;

namespace RozeCare.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(Guid? actorId, string action, string resourceType, Guid? resourceId, object details, CancellationToken cancellationToken = default)
    {
        var log = new AuditLog
        {
            ActorUserId = actorId,
            Action = action,
            ResourceType = resourceType,
            ResourceId = resourceId,
            Details = JsonSerializer.Serialize(details),
            WhenUtc = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
