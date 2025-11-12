using FluentValidation;
using MediatR;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;

namespace RozeCare.Application.Encounters.Commands;

public record CreateEncounterCommand(Guid PatientId, Guid ProviderId, DateTime Date, string Type, string Notes, IReadOnlyCollection<string> Diagnoses, IReadOnlyCollection<string> Prescriptions) : IRequest<Guid>;

public class CreateEncounterCommandValidator : AbstractValidator<CreateEncounterCommand>
{
    public CreateEncounterCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.ProviderId).NotEmpty();
        RuleFor(x => x.Type).NotEmpty();
    }
}

public class CreateEncounterCommandHandler : IRequestHandler<CreateEncounterCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateEncounterCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateEncounterCommand request, CancellationToken cancellationToken)
    {
        var encounter = new Encounter
        {
            PatientId = request.PatientId,
            ProviderId = request.ProviderId,
            Date = request.Date,
            Type = request.Type,
            Notes = request.Notes,
            Diagnoses = request.Diagnoses.ToList(),
            Prescriptions = request.Prescriptions.ToList()
        };

        _context.Encounters.Add(encounter);
        await _context.SaveChangesAsync(cancellationToken);
        return encounter.Id;
    }
}
