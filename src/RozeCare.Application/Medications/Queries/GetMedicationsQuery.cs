using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Medications.Queries;

public record MedicationDto(Guid Id, string Name, string Dosage, DateTime StartDate, DateTime? EndDate, string? PrescribedBy);

public record GetMedicationsQuery(Guid PatientId) : IRequest<IReadOnlyList<MedicationDto>>;

public class GetMedicationsQueryHandler : IRequestHandler<GetMedicationsQuery, IReadOnlyList<MedicationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMedicationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MedicationDto>> Handle(GetMedicationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Medications.AsNoTracking()
            .Where(m => m.PatientId == request.PatientId)
            .ProjectTo<MedicationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
