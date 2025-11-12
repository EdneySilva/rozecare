using FluentValidation;
using MediatR;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;

namespace RozeCare.Application.Observations.Commands;

public record CreateObservationCommand(Guid PatientId, string Code, string Display, string? ValueString, decimal? ValueQuantity, string? ValueCodeable, string? Unit, DateTime EffectiveDate) : IRequest<Guid>;

public class CreateObservationCommandValidator : AbstractValidator<CreateObservationCommand>
{
    public CreateObservationCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Display).NotEmpty();
    }
}

public class CreateObservationCommandHandler : IRequestHandler<CreateObservationCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateObservationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateObservationCommand request, CancellationToken cancellationToken)
    {
        var observation = new Observation
        {
            PatientId = request.PatientId,
            Code = request.Code,
            Display = request.Display,
            ValueString = request.ValueString,
            ValueQuantity = request.ValueQuantity,
            ValueCodeable = request.ValueCodeable,
            Unit = request.Unit,
            EffectiveDate = request.EffectiveDate
        };

        _context.Observations.Add(observation);
        await _context.SaveChangesAsync(cancellationToken);
        return observation.Id;
    }
}
