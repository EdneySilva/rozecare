using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Fhir.Queries;

public record FhirObservationDto(Guid Id, string Code, string Display, string? ValueString, decimal? ValueQuantity, string? Unit, DateTime EffectiveDate);

public record FhirBundle<T>(string ResourceType, string Type, IReadOnlyList<T> Entry);

public record GetFhirObservationBundleQuery(Guid PatientId) : IRequest<FhirBundle<FhirObservationDto>>;

public class GetFhirObservationBundleQueryHandler : IRequestHandler<GetFhirObservationBundleQuery, FhirBundle<FhirObservationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFhirObservationBundleQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FhirBundle<FhirObservationDto>> Handle(GetFhirObservationBundleQuery request, CancellationToken cancellationToken)
    {
        var observations = await _context.Observations.AsNoTracking()
            .Where(o => o.PatientId == request.PatientId)
            .Select(o => new FhirObservationDto(o.Id, o.Code, o.Display, o.ValueString, o.ValueQuantity, o.Unit, o.EffectiveDate))
            .ToListAsync(cancellationToken);

        return new FhirBundle<FhirObservationDto>("Bundle", "collection", observations);
    }
}
