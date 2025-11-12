using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Encounters.Queries;

public record EncounterDto(Guid Id, Guid ProviderId, DateTime Date, string Type, string Notes, IReadOnlyList<string> Diagnoses, IReadOnlyList<string> Prescriptions);

public record GetEncountersQuery(Guid PatientId) : IRequest<IReadOnlyList<EncounterDto>>;

public class GetEncountersQueryHandler : IRequestHandler<GetEncountersQuery, IReadOnlyList<EncounterDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEncountersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EncounterDto>> Handle(GetEncountersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Encounters.AsNoTracking()
            .Where(e => e.PatientId == request.PatientId)
            .ProjectTo<EncounterDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
