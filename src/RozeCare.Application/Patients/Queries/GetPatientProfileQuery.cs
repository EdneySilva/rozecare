using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Application.Common.Models;

namespace RozeCare.Application.Patients.Queries;

public record GetPatientProfileQuery(Guid PatientId) : IRequest<PatientProfileDto?>;

public class GetPatientProfileQueryHandler : IRequestHandler<GetPatientProfileQuery, PatientProfileDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPatientProfileQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientProfileDto?> Handle(GetPatientProfileQuery request, CancellationToken cancellationToken)
    {
        return await _context.PatientProfiles
            .AsNoTracking()
            .Where(p => p.UserId == request.PatientId)
            .ProjectTo<PatientProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
