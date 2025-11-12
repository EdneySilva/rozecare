using FluentValidation;
using MediatR;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Authentication.Commands;

public record LoginCommand(string Email, string Password) : IRequest<TokenResult>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResult>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginCommandHandler(IIdentityService identityService, IJwtTokenGenerator tokenGenerator)
    {
        _identityService = identityService;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<TokenResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByEmailAsync(request.Email, cancellationToken)
            ?? throw new InvalidOperationException("Invalid credentials.");

        var valid = await _identityService.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        return await _tokenGenerator.GenerateTokensAsync(user, cancellationToken);
    }
}
