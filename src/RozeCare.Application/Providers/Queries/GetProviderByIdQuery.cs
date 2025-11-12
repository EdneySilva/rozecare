using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Providers.Queries;

public record GetProviderByIdQuery(Guid ProviderId) : IRequest<ProviderDto?>;

public class GetProviderByIdQueryHandler : IRequestHandler<GetProviderByIdQuery, ProviderDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProviderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProviderDto?> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers.AsNoTracking()
            .Where(p => p.Id == request.ProviderId)
            .ProjectTo<ProviderDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
