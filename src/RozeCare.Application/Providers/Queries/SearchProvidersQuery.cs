using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Providers.Queries;

public record ProviderDto(Guid Id, string Name, string Type, string Address, string Contact, string? Accreditation);

public record SearchProvidersQuery(string? Text) : IRequest<IReadOnlyList<ProviderDto>>;

public class SearchProvidersQueryHandler : IRequestHandler<SearchProvidersQuery, IReadOnlyList<ProviderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchProvidersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProviderDto>> Handle(SearchProvidersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Providers.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Text))
        {
            query = query.Where(p => p.Name.ToLower().Contains(request.Text.ToLower()));
        }

        return await query.ProjectTo<ProviderDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}
