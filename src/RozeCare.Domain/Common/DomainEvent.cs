using MediatR;

namespace RozeCare.Domain.Common;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
