using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class UserRefreshToken : BaseAuditableEntity
{
    public Guid UserId { get; set; }
        = Guid.Empty;

    public ApplicationUser? User { get; set; }
        = null;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; } = DateTime.UtcNow.AddDays(7);

    public bool IsRevoked { get; set; }
        = false;

    public static UserRefreshToken Create(Guid userId, string token, DateTime expiresAtUtc)
        => new()
        {
            UserId = userId,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        };
}
