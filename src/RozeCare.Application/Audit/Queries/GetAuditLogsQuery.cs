using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Audit.Queries;

public record AuditLogDto(Guid Id, DateTime WhenUtc, Guid? ActorUserId, string Action, string ResourceType, Guid? ResourceId, string Details);

public record GetAuditLogsQuery(Guid? PatientId, Guid? ActorId, DateTime? From, DateTime? To) : IRequest<IReadOnlyList<AuditLogDto>>;

public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, IReadOnlyList<AuditLogDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAuditLogsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AuditLogDto>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.AuditLogs.AsNoTracking();

        if (request.PatientId is not null)
        {
            var patientIdString = request.PatientId.Value.ToString();
            query = query.Where(a => a.Details.Contains(patientIdString));
        }

        if (request.ActorId is not null)
        {
            query = query.Where(a => a.ActorUserId == request.ActorId);
        }

        if (request.From is not null)
        {
            query = query.Where(a => a.WhenUtc >= request.From);
        }

        if (request.To is not null)
        {
            query = query.Where(a => a.WhenUtc <= request.To);
        }

        return await query.OrderByDescending(a => a.WhenUtc)
            .ProjectTo<AuditLogDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
