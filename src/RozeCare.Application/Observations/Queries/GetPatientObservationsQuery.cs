using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Application.Common.Models;

namespace RozeCare.Application.Observations.Queries;

public record ObservationDto(Guid Id, string Code, string Display, string? ValueString, decimal? ValueQuantity, string? ValueCodeable, string? Unit, DateTime EffectiveDate);

public record GetPatientObservationsQuery(Guid PatientId, string? Code, DateTime? DateFrom, DateTime? DateTo) : IRequest<IReadOnlyList<ObservationDto>>;

public class GetPatientObservationsQueryHandler : IRequestHandler<GetPatientObservationsQuery, IReadOnlyList<ObservationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPatientObservationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ObservationDto>> Handle(GetPatientObservationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Observations.AsNoTracking().Where(o => o.PatientId == request.PatientId);

        if (!string.IsNullOrEmpty(request.Code))
        {
            query = query.Where(o => o.Code == request.Code);
        }

        if (request.DateFrom is not null)
        {
            query = query.Where(o => o.EffectiveDate >= request.DateFrom);
        }

        if (request.DateTo is not null)
        {
            query = query.Where(o => o.EffectiveDate <= request.DateTo);
        }

        return await query.ProjectTo<ObservationDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}
