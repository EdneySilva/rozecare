using FluentValidation;
using MediatR;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;
using RozeCare.Domain.Enums;

namespace RozeCare.Application.Providers.Commands;

public record CreateProviderCommand(string Name, ProviderType Type, string Address, string Contact, string? Accreditation) : IRequest<Guid>;

public class CreateProviderCommandValidator : AbstractValidator<CreateProviderCommand>
{
    public CreateProviderCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Contact).NotEmpty();
    }
}

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProviderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = new Provider
        {
            Name = request.Name,
            Type = request.Type,
            Address = request.Address,
            Contact = request.Contact,
            Accreditation = request.Accreditation
        };

        _context.Providers.Add(provider);
        await _context.SaveChangesAsync(cancellationToken);
        return provider.Id;
    }
}
