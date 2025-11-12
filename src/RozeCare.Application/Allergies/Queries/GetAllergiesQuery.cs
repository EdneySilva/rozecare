using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Allergies.Queries;

public record AllergyDto(Guid Id, string Substance, string Reaction, string Severity);

public record GetAllergiesQuery(Guid PatientId) : IRequest<IReadOnlyList<AllergyDto>>;

public class GetAllergiesQueryHandler : IRequestHandler<GetAllergiesQuery, IReadOnlyList<AllergyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllergiesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AllergyDto>> Handle(GetAllergiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Allergies.AsNoTracking()
            .Where(a => a.PatientId == request.PatientId)
            .ProjectTo<AllergyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
