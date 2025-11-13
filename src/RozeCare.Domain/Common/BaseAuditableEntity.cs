using System;
using System.Collections.Generic;

namespace RozeCare.Domain.Common;

public abstract class BaseAuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Guid? CreatedBy { get; set; }
        = null;

    public DateTime? UpdatedAtUtc { get; set; }
        = null;

    public Guid? UpdatedBy { get; set; }
        = null;
    // TODO: AVALIAR como utilizar essa propriedade
    //public List<DomainEvent> DomainEvents { get; } = new();
}
