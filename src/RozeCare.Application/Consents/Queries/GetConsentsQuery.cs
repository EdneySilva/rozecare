using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Enums;

namespace RozeCare.Application.Consents.Queries;

public record ConsentDto(Guid Id, Guid PatientId, ConsentGranteeType GranteeType, Guid GranteeId, IReadOnlyList<string> Scopes, DateTime ExpiresAtUtc, ConsentStatus Status);

public record GetConsentsQuery(Guid PatientId) : IRequest<IReadOnlyList<ConsentDto>>;

public class GetConsentsQueryHandler : IRequestHandler<GetConsentsQuery, IReadOnlyList<ConsentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetConsentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ConsentDto>> Handle(GetConsentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Consents.AsNoTracking()
            .Where(c => c.PatientId == request.PatientId)
            .ProjectTo<ConsentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
