using FluentValidation;
using MediatR;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;

namespace RozeCare.Application.Allergies.Commands;

public record CreateAllergyCommand(Guid PatientId, string Substance, string Reaction, string Severity) : IRequest<Guid>;

public class CreateAllergyCommandValidator : AbstractValidator<CreateAllergyCommand>
{
    public CreateAllergyCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.Substance).NotEmpty();
        RuleFor(x => x.Reaction).NotEmpty();
        RuleFor(x => x.Severity).NotEmpty();
    }
}

public class CreateAllergyCommandHandler : IRequestHandler<CreateAllergyCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateAllergyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAllergyCommand request, CancellationToken cancellationToken)
    {
        var allergy = new Allergy
        {
            PatientId = request.PatientId,
            Substance = request.Substance,
            Reaction = request.Reaction,
            Severity = request.Severity
        };

        _context.Allergies.Add(allergy);
        await _context.SaveChangesAsync(cancellationToken);
        return allergy.Id;
    }
}
