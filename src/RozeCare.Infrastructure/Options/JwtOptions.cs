namespace RozeCare.Infrastructure.Options;

public class JwtOptions
{
    public string? Authority { get; set; }
    public string? Audience { get; set; }
    public string? SigningKey { get; set; }
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;
}
