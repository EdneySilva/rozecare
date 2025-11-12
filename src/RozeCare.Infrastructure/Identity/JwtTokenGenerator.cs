using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;
using RozeCare.Infrastructure.Options;
using RozeCare.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RozeCare.Infrastructure.Identity;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;
    private readonly ApplicationDbContext _context;

    public JwtTokenGenerator(IOptions<JwtOptions> options, ApplicationDbContext context)
    {
        _options = options.Value;
        _context = context;
    }

    public async Task<TokenResult> GenerateTokensAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey ?? "dev-secret"));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new("role", user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _options.Authority,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_options.AccessTokenMinutes),
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var refresh = UserRefreshToken.Create(user.Id, refreshToken, now.AddDays(_options.RefreshTokenDays));
        _context.RefreshTokens.Add(refresh);
        await _context.SaveChangesAsync(cancellationToken);

        return new TokenResult(accessToken, refreshToken, token.ValidTo);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        var existing = _context.RefreshTokens.FirstOrDefault(r => r.UserId == userId && r.Token == refreshToken);
        if (existing is null)
        {
            return;
        }

        existing.IsRevoked = true;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
