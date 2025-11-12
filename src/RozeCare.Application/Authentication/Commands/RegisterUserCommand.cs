using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;
using RozeCare.Domain.Enums;

namespace RozeCare.Application.Authentication.Commands;

public record RegisterUserCommand(string Email, string Password, string Name, UserRole Role) : IRequest<TokenResult>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenResult>
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RegisterUserCommandHandler(
        IIdentityService identityService,
        IApplicationDbContext context,
        IJwtTokenGenerator tokenGenerator)
    {
        _identityService = identityService;
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<TokenResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("User already exists.");
        }

        var user = await _identityService.CreateUserAsync(request.Email, request.Password, request.Name, request.Role.ToString(), cancellationToken);
        user.Role = request.Role;
        await _context.SaveChangesAsync(cancellationToken);

        if (request.Role == UserRole.Patient)
        {
            var profile = new PatientProfile
            {
                UserId = user.Id
            };
            _context.PatientProfiles.Add(profile);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return await _tokenGenerator.GenerateTokensAsync(user, cancellationToken);
    }
}
