using FluentValidation;
using MediatR;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;

namespace RozeCare.Application.Medications.Commands;

public record CreateMedicationCommand(Guid PatientId, string Name, string Dosage, DateTime StartDate, DateTime? EndDate, string? PrescribedBy) : IRequest<Guid>;

public class CreateMedicationCommandValidator : AbstractValidator<CreateMedicationCommand>
{
    public CreateMedicationCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Dosage).NotEmpty();
    }
}

public class CreateMedicationCommandHandler : IRequestHandler<CreateMedicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateMedicationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMedicationCommand request, CancellationToken cancellationToken)
    {
        var medication = new Medication
        {
            PatientId = request.PatientId,
            Name = request.Name,
            Dosage = request.Dosage,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PrescribedBy = request.PrescribedBy
        };

        _context.Medications.Add(medication);
        await _context.SaveChangesAsync(cancellationToken);
        return medication.Id;
    }
}
