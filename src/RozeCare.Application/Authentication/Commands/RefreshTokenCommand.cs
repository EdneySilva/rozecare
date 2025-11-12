using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Authentication.Commands;

public record RefreshTokenCommand(Guid UserId, string RefreshToken) : IRequest<TokenResult>;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RefreshTokenCommandHandler(IApplicationDbContext context, IJwtTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<TokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == request.UserId && t.Token == request.RefreshToken && !t.IsRevoked, cancellationToken);

        if (token is null || token.ExpiresAtUtc < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Invalid refresh token.");
        }

        token.IsRevoked = true;
        await _context.SaveChangesAsync(cancellationToken);

        return await _tokenGenerator.GenerateTokensAsync(token.User!, cancellationToken);
    }
}
