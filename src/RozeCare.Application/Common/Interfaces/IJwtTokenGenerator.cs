using RozeCare.Domain.Entities;

namespace RozeCare.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<TokenResult> GenerateTokensAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);
}

public record TokenResult(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);
