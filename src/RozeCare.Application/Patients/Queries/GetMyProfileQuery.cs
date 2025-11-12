using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Patients.Queries;

public record PatientProfileDto(Guid UserId, string? BloodType, IReadOnlyList<string> Conditions, IReadOnlyList<string> Allergies, IReadOnlyList<string> EmergencyContacts, IReadOnlyList<string> PreferredProviders);

public record GetMyProfileQuery(Guid UserId) : IRequest<PatientProfileDto?>;

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, PatientProfileDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMyProfileQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientProfileDto?> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        return await _context.PatientProfiles
            .AsNoTracking()
            .Where(p => p.UserId == request.UserId)
            .ProjectTo<PatientProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
