using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Authentication.Commands;

public record LogoutCommand(Guid UserId, string RefreshToken) : IRequest<Unit>;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public LogoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == request.UserId && t.Token == request.RefreshToken, cancellationToken);
        if (token is null)
        {
            return Unit.Value;
        }

        token.IsRevoked = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
